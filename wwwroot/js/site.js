document.addEventListener("DOMContentLoaded", function () {

    // 1. STATS COUNTER LOGIC
    const statsObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const targetEl = entry.target;
                const targetValue = +targetEl.getAttribute('data-target');
                let currentCount = 0;

                statsObserver.unobserve(targetEl);

                const updateCount = () => {
                    const increment = targetValue / 50;
                    if (currentCount < targetValue) {
                        currentCount = Math.ceil(currentCount + increment);
                        targetEl.innerText = currentCount;
                        setTimeout(updateCount, 20);
                    } else {
                        targetEl.innerText = targetValue;
                    }
                };
                updateCount();
            }
        });
    }, { threshold: 0.5 });

    document.querySelectorAll('.stat-number').forEach(num => statsObserver.observe(num));


    // 2. SCROLL-IN REVEAL ANIMATION
    const revealObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('is-visible');
                revealObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.2 });

    document.querySelectorAll('.fade-in-up').forEach(el => revealObserver.observe(el));


    // 3. SERVICES GRID HOVER
    const serviceCards = document.querySelectorAll('#services-grid .service-card');
    serviceCards.forEach(card => {
        const btn = card.querySelector('.btn');
        if (!btn) return;

        const originalText = btn.textContent.trim();

        card.addEventListener('mouseenter', () => {
            btn.textContent = 'Get Started';
            btn.classList.replace('btn-outline-success', 'btn-success');
        });

        card.addEventListener('mouseleave', () => {
            btn.textContent = originalText;
            btn.classList.replace('btn-success', 'btn-outline-success');
        });
    });

    // 4. MEAL PLAN PACKAGES HOVER
    const pricingCards = document.querySelectorAll('#packages .card');
    pricingCards.forEach(card => {
        const btn = card.querySelector('.btn');
        if (!btn) return;

        const originalText = btn.textContent.trim();

        card.addEventListener('mouseenter', () => {
            btn.textContent = 'Get Started';
            if (btn.classList.contains('btn-outline-dark')) {
                btn.classList.replace('btn-outline-dark', 'btn-success');
            }
        });

        card.addEventListener('mouseleave', () => {
            btn.textContent = originalText;
            if (btn.classList.contains('btn-success')) {
                btn.classList.replace('btn-success', 'btn-outline-dark');
            }
        });
    });
});

// 5. TESTIMONIAL SLIDER (JQUERY / SLICK)
// Keep this slightly separate to ensure jQuery loads first
$(document).ready(function () {
    $('.testimonial-slider').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000,
        dots: true,
        arrows: true,
        infinite: true,
        prevArrow: '<button type="button" class="slick-prev shadow-sm"><i class="bi bi-chevron-left"></i></button>',
        nextArrow: '<button type="button" class="slick-right shadow-sm"><i class="bi bi-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 1100,
                settings: { slidesToShow: 2 }
            },
            {
                breakpoint: 768,
                settings: { slidesToShow: 1 }
            }
        ]
    });
});