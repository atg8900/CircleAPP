// 1. Dark Mode Logic (يفضل أن يظل في الأعلى لضمان سرعة التنفيذ)
if (localStorage.theme === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
    document.documentElement.classList.add('dark');
} else {
    document.documentElement.classList.remove('dark');
}

// 2. وظائف تغيير الثيم (يمكنك مناداتها من أزرار الـ HTML)
function setTheme(mode) {
    if (mode === 'dark') {
        localStorage.theme = 'dark';
        document.documentElement.classList.add('dark');
    } else if (mode === 'light') {
        localStorage.theme = 'light';
        document.documentElement.classList.remove('dark');
    } else {
        localStorage.removeItem('theme');
        if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    }
}

// 3. كود المعاينة والعمليات بعد تحميل الصفحة
document.addEventListener('DOMContentLoaded', function () {

    // دالة ذكية لمعاينة الصور (تمنع تكرار الكود)
    function handleImagePreview(inputId, imageId) {
        const input = document.getElementById(inputId);
        const preview = document.getElementById(imageId);

        if (input && preview) {
            input.addEventListener('change', function () {
                if (this.files && this.files[0]) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        preview.setAttribute('src', e.target.result);
                        preview.style.display = 'block';
                    };
                    reader.readAsDataURL(this.files[0]);
                }
            });
        }
    }

    // تفعيل المعاينة لكل الأقسام (لو العنصر مش موجود مش هيطلع Error)
    handleImagePreview('addPostUrl', 'addPostImage');           // إضافة بوست
    handleImagePreview('createStatusUrl', 'createStatusImage');   // إضافة حالة (Status)
    handleImagePreview('createProductUrl', 'createProductImage'); // إضافة منتج
});

// 4. دالة حذف المنشور (خارج الـ DOMContentLoaded عشان الـ HTML يشوفها)
function openPostDeleteConfirmation(postId) {
    // إخفاء القائمة المنسدلة (Dropdown)
    const dropdown = document.querySelector('.post-options-dropdown');
    if (dropdown) {
        try {
            UIkit.dropdown(dropdown).hide(false);
        } catch (e) {
            console.warn("UIkit dropdown not found or already hidden.");
        }
    }

    // تعيين الـ ID في الحقل المخفي داخل المودال
    const postIdInput = document.getElementById('deletConfirmationPostId');
    if (postIdInput) {
        postIdInput.value = postId;
    }

    // إظهار نافذة التأكيد (Modal)
    const modal = document.getElementById('postDeleteDialog');
    if (modal) {
        UIkit.modal(modal).show();
    }
}