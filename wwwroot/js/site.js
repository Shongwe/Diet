document.addEventListener("DOMContentLoaded", function () {
    // 1. Scroll-in Animation Logic
    const observerOptions = { threshold: 0.2 };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('is-visible');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    document.querySelectorAll('.fade-in-up').forEach(el => {
        observer.observe(el);
    });

    // 2. Services Grid Hover Logic (Nutrition Therapy Cards)
    // Targets: #services-grid .service-card
    const gridCards = document.querySelectorAll('#services-grid .service-card');
    gridCards.forEach(card => {
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

    // 3. Meal Plan Packages Hover Logic (Pricing Cards)
    // Targets: #services .card (The 3-column pricing section)
    const pricingCards = document.querySelectorAll('#services .card');
    pricingCards.forEach(card => {
        const btn = card.querySelector('.btn');
        if (!btn) return;

        const originalText = btn.textContent.trim();

        card.addEventListener('mouseenter', () => {
            btn.textContent = 'Get Started';
            // Switches 'Learn More' (outline) to 'Get Started' (solid green)
            if (btn.classList.contains('btn-outline-dark')) {
                btn.classList.replace('btn-outline-dark', 'btn-success');
            }
        });

        card.addEventListener('mouseleave', () => {
            btn.textContent = originalText;
            // Only revert color if it was originally an outline button
            if (originalText === 'Learn More') {
                btn.classList.replace('btn-success', 'btn-outline-dark');
            }
        });
    });
});