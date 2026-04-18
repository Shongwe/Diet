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


    // 3. SERVICES & PACKAGES HOVER LOGIC (Combined for Efficiency)
    const setupHover = (selector, outlineClass) => {
        document.querySelectorAll(selector).forEach(card => {
            const btn = card.querySelector('.btn');
            if (!btn) return;
            const originalText = btn.textContent.trim();

            card.addEventListener('mouseenter', () => {
                btn.textContent = 'Get Started';
                btn.classList.replace(outlineClass, 'btn-success');
            });
            card.addEventListener('mouseleave', () => {
                btn.textContent = originalText;
                btn.classList.replace('btn-success', outlineClass);
            });
        });
    };

    setupHover('#services-grid .service-card', 'btn-outline-success');
    setupHover('#packages .card', 'btn-outline-dark');
});

// 4. TESTIMONIAL SLIDER (JQUERY / SLICK)
$(document).ready(function () {
    const $slider = $('.testimonial-slider');
    if ($slider.length) {
        $slider.slick({
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
                { breakpoint: 1100, settings: { slidesToShow: 2 } },
                { breakpoint: 768, settings: { slidesToShow: 1 } }
            ]
        });
    }
});

// 5. BIO TOGGLE (Accessible Globally)
function toggleBio() {
    const dots = document.getElementById("dots");
    const moreText = document.getElementById("more-bio");
    const btnText = document.getElementById("bioBtn");

    if (!dots || !moreText || !btnText) return;

    if (dots.style.display === "none") {
        dots.style.display = "inline";
        btnText.innerHTML = "See More";
        moreText.style.display = "none";
    } else {
        dots.style.display = "none";
        btnText.innerHTML = "See Less";
        moreText.style.display = "inline";
    }
}