document.addEventListener('DOMContentLoaded', function () {
    var productImagePlaceholder = '/images/products/no-image.png';

    document.addEventListener('error', function (event) {
        var img = event.target;

        if (!img || img.tagName !== 'IMG' || !img.src) {
            return;
        }

        if (img.src.indexOf('/images/products/') === -1 || img.src.indexOf('no-image.png') !== -1) {
            return;
        }

        img.onerror = null;
        img.src = productImagePlaceholder;
    }, true);

    document.querySelectorAll('[data-password-toggle]').forEach(function (button) {
        button.addEventListener('click', function () {
            var selector = button.getAttribute('data-password-toggle');
            var input = document.querySelector(selector);

            if (!input) {
                return;
            }

            var isHidden = input.getAttribute('type') === 'password';
            input.setAttribute('type', isHidden ? 'text' : 'password');
            button.setAttribute('aria-label', isHidden ? 'Скрыть пароль' : 'Показать пароль');
        });
    });

    var anchorLinks = document.querySelectorAll('.store-nav-anchor[data-nav-anchor]');
    if (anchorLinks.length > 0) {
        var setActiveAnchor = function () {
            var hash = window.location.hash.replace('#', '');
            anchorLinks.forEach(function (link) {
                link.classList.toggle('active', hash && link.getAttribute('data-nav-anchor') === hash);
            });
        };

        setActiveAnchor();
        window.addEventListener('hashchange', setActiveAnchor);
    }
});
