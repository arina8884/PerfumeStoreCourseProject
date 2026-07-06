param(
    [string]$BaseUrl = "http://localhost:5084"
)

$ErrorPatterns = @(
    'InvalidOperationException',
    'NullReferenceException',
    'have been defined but have not been rendered',
    'An unhandled exception occurred',
    'Internal Server Error',
    'RazorRuntimeException',
    'Developer Exception Page'
)

function New-SessionWithLogin {
    param([string]$Email, [string]$Password)
    $session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
    try {
        $loginPage = Invoke-WebRequest -Uri "$BaseUrl/account/login" -WebSession $session -UseBasicParsing -TimeoutSec 15
    } catch {
        return $null
    }
    $token = [regex]::Match($loginPage.Content, 'name="__RequestVerificationToken" type="hidden" value="([^"]+)"').Groups[1].Value
    if (-not $token) { return $null }
    $body = @{ Email = $Email; Password = $Password; __RequestVerificationToken = $token }
    try {
        Invoke-WebRequest -Uri "$BaseUrl/account/login" -Method POST -WebSession $session -Body $body -UseBasicParsing -TimeoutSec 15 | Out-Null
    } catch { }
    return $session
}

function Test-Route {
    param($Session, [string]$Path, [string]$Role, [int[]]$OkStatuses = @(200, 302))
    $url = if ($Path.StartsWith('http')) { $Path } else { "$BaseUrl$Path" }
    $result = [ordered]@{ Role = $Role; Path = $Path; Status = 0; Issue = $null }
    try {
        $r = Invoke-WebRequest -Uri $url -WebSession $Session -UseBasicParsing -MaximumRedirection 5 -TimeoutSec 20 -ErrorAction Stop
        $result.Status = [int]$r.StatusCode
        $content = $r.Content
    } catch {
        $resp = $_.Exception.Response
        if ($resp) {
            $result.Status = [int]$resp.StatusCode
            $reader = New-Object System.IO.StreamReader($resp.GetResponseStream())
            $content = $reader.ReadToEnd()
            $reader.Close()
        } else {
            $result.Issue = $_.Exception.Message
            return $result
        }
    }
    if ($result.Status -eq 500) {
        $result.Issue = 'HTTP 500'
    } elseif ($result.Status -notin $OkStatuses) {
        $result.Issue = "HTTP $($result.Status)"
    }
    foreach ($p in $ErrorPatterns) {
        if ($content -and $content -match [regex]::Escape($p)) {
            $result.Issue = "Content error: $p"
            break
        }
    }
    return $result
}

# Discover IDs from guest catalog
$guest = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$catalog = Invoke-WebRequest "$BaseUrl/catalog" -WebSession $guest -UseBasicParsing
$productIds = [regex]::Matches($catalog.Content, '/product/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
$categoryIds = [regex]::Matches($catalog.Content, '/catalog/category/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
$productId = if ($productIds) { $productIds[0] } else { 1 }
$categoryId = if ($categoryIds) { $categoryIds[0] } else { 1 }

$guestRoutes = @(
    '/', '/catalog', '/catalog/search', "/catalog/category/$categoryId", "/product/$productId",
    '/account/login', '/account/register', '/account/access-denied', '/Home/Error'
)

$clientRoutes = @(
    '/client', '/client/profile', '/client/profile/edit', '/client/orders',
    '/client/favorites', '/client/reviews', '/compare', '/cart',
    '/checkout', '/checkout/delivery', '/checkout/payment', '/checkout/confirm'
)

$cmRoutes = @(
    '/content-manager', '/content-manager/products', '/content-manager/products/archived',
    '/content-manager/products/create', "/content-manager/products/$productId/edit",
    "/content-manager/products/$productId/preview",
    '/content-manager/categories', '/content-manager/categories/create',
    "/content-manager/categories/$categoryId/edit"
)

$omRoutes = @(
    '/order-manager', '/order-manager/orders', '/order-manager/orders/queue',
    '/order-manager/orders/history', '/order-manager/warehouse',
    '/order-manager/suppliers', '/order-manager/suppliers/create',
    '/order-manager/supply-requests', '/order-manager/supply-requests/create'
)

$adminRoutes = @(
    '/admin', '/admin/users', '/admin/users/create', '/admin/reports',
    '/admin/reports/export/excel', '/admin/reports/export/pdf', '/admin/reports/export/word'
)

$roles = @(
    @{ Name = 'Guest'; Session = $guest; Routes = $guestRoutes }
    @{ Name = 'Client'; Email = 'anna.koval@mail.by'; Password = 'client123'; Routes = $guestRoutes + $clientRoutes }
    @{ Name = 'ContentManager'; Email = 'content@perfume.by'; Password = 'content123'; Routes = $cmRoutes }
    @{ Name = 'OrderManager'; Email = 'manager@perfume.by'; Password = 'manager123'; Routes = $omRoutes }
    @{ Name = 'Admin'; Email = 'admin@perfume.by'; Password = 'admin123'; Routes = $adminRoutes }
)

$issues = @()
foreach ($role in $roles) {
    $session = if ($role.Session) { $role.Session } else { New-SessionWithLogin $role.Email $role.Password }
    if (-not $session -and $role.Name -ne 'Guest') {
        $issues += [pscustomobject]@{ Role = $role.Name; Path = 'LOGIN'; Status = 0; Issue = 'Login failed' }
        continue
    }
    foreach ($path in $role.Routes) {
        $r = Test-Route -Session $session -Path $path -Role $role.Name
        if ($r.Issue) { $issues += [pscustomobject]$r }
    }
}

# Client order details if orders exist
$clientSession = New-SessionWithLogin 'anna.koval@mail.by' 'client123'
if ($clientSession) {
    $ordersPage = Invoke-WebRequest "$BaseUrl/client/orders" -WebSession $clientSession -UseBasicParsing
    $orderIds = [regex]::Matches($ordersPage.Content, '/client/orders/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($oid in $orderIds) {
        $r = Test-Route -Session $clientSession -Path "/client/orders/$oid" -Role 'Client'
        if ($r.Issue) { $issues += [pscustomobject]$r }
        $r2 = Test-Route -Session $clientSession -Path "/client/orders/$oid/tracking" -Role 'Client'
        if ($r2.Issue) { $issues += [pscustomobject]$r2 }
    }
    $reviewsPage = Invoke-WebRequest "$BaseUrl/client/reviews" -WebSession $clientSession -UseBasicParsing
    $reviewIds = [regex]::Matches($reviewsPage.Content, '/client/reviews/(\d+)/edit') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($rid in $reviewIds) {
        $r = Test-Route -Session $clientSession -Path "/client/reviews/$rid/edit" -Role 'Client'
        if ($r.Issue) { $issues += [pscustomobject]$r }
    }
}

# OM order/supplier details
$omSession = New-SessionWithLogin 'manager@perfume.by' 'manager123'
if ($omSession) {
    $orders = Invoke-WebRequest "$BaseUrl/order-manager/orders" -WebSession $omSession -UseBasicParsing
    $omOrderIds = [regex]::Matches($orders.Content, '/order-manager/orders/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($oid in $omOrderIds) {
        foreach ($suffix in @('', '/status', '/cancel')) {
            $r = Test-Route -Session $omSession -Path "/order-manager/orders/$oid$suffix" -Role 'OrderManager'
            if ($r.Issue) { $issues += [pscustomobject]$r }
        }
    }
    $suppliers = Invoke-WebRequest "$BaseUrl/order-manager/suppliers" -WebSession $omSession -UseBasicParsing
    $supIds = [regex]::Matches($suppliers.Content, '/order-manager/suppliers/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($sid in $supIds) {
        $r = Test-Route -Session $omSession -Path "/order-manager/suppliers/$sid" -Role 'OrderManager'
        if ($r.Issue) { $issues += [pscustomobject]$r }
        $r2 = Test-Route -Session $omSession -Path "/order-manager/suppliers/$sid/edit" -Role 'OrderManager'
        if ($r2.Issue) { $issues += [pscustomobject]$r2 }
    }
    $requests = Invoke-WebRequest "$BaseUrl/order-manager/supply-requests" -WebSession $omSession -UseBasicParsing
    $reqIds = [regex]::Matches($requests.Content, '/order-manager/supply-requests/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($rid in $reqIds) {
        $r = Test-Route -Session $omSession -Path "/order-manager/supply-requests/$rid" -Role 'OrderManager'
        if ($r.Issue) { $issues += [pscustomobject]$r }
    }
}

# Admin user details
$adminSession = New-SessionWithLogin 'admin@perfume.by' 'admin123'
if ($adminSession) {
    $users = Invoke-WebRequest "$BaseUrl/admin/users" -WebSession $adminSession -UseBasicParsing
    $userIds = [regex]::Matches($users.Content, '/admin/users/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    $editIds = [regex]::Matches($users.Content, '/admin/users/edit/(\d+)') | ForEach-Object { $_.Groups[1].Value } | Select-Object -Unique
    foreach ($uid in $userIds) {
        $r = Test-Route -Session $adminSession -Path "/admin/users/$uid" -Role 'Admin'
        if ($r.Issue) { $issues += [pscustomobject]$r }
    }
    foreach ($uid in $editIds) {
        $r = Test-Route -Session $adminSession -Path "/admin/users/edit/$uid" -Role 'Admin'
        if ($r.Issue) { $issues += [pscustomobject]$r }
    }
}

if ($issues.Count -eq 0) {
    Write-Output 'ALL_ROUTES_OK'
} else {
    Write-Output "ISSUES: $($issues.Count)"
    $issues | Format-Table -AutoSize | Out-String | Write-Output
}
