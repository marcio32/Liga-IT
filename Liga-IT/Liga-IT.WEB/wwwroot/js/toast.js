class Toast {
    constructor() {
        this.container = this.getOrCreateContainer();
    }

    getOrCreateContainer() {
        let container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
        }
        return container;
    }

    show(message, type = 'success', duration = 10000) {
        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        
        toast.innerHTML = `
            <div class="toast-content">
                <span class="toast-message">${this.escapeHtml(message)}</span>
                <button class="toast-close" aria-label="Cerrar">
                    <i class="fas fa-times"></i>
                </button>
            </div>
            <div class="toast-progress"></div>
        `;

        this.container.appendChild(toast);

        const closeBtn = toast.querySelector('.toast-close');
        closeBtn.addEventListener('click', () => this.remove(toast));

        setTimeout(() => this.remove(toast), duration);
    }

    remove(toast) {
        toast.classList.add('removing');
        setTimeout(() => toast.remove(), 300);
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    success(message) {
        this.show(message, 'success');
    }

    warning(message) {
        this.show(message, 'warning');
    }

    error(message) {
        this.show(message, 'error');
    }
}

const toast = new Toast();
