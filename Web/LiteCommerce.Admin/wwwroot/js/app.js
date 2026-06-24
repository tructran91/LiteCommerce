if (window.QuillFunctions) {
    var origLoadContent = window.QuillFunctions.loadQuillContent;
    window.QuillFunctions.loadQuillContent = function (quillElement, quillContent) {
        if (quillElement.__quill) {
            quillElement.__quill.clipboard.dangerouslyPasteHTML(quillContent);
        }
    };
}

window.QuillImageUpload = {
    registerImageHandler: function (quillContainerSelector, dotNetHelper) {
        var container = document.querySelector(quillContainerSelector);
        if (!container) return;
        var quillEditor = container.querySelector('.ql-editor');
        if (!quillEditor) return;
        var quillContainer = container.querySelector('.ql-container');
        if (!quillContainer || !quillContainer.__quill) return;

        var quill = quillContainer.__quill;
        var toolbar = quill.getModule('toolbar');
        toolbar.addHandler('image', function () {
            var input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'image/*');
            input.click();

            input.onchange = async function () {
                var file = input.files[0];
                if (!file) return;

                var reader = new FileReader();
                reader.onload = async function (e) {
                    var base64 = e.target.result.split(',')[1];
                    var result = await dotNetHelper.invokeMethodAsync('UploadContentImage', file.name, file.type, base64);
                    if (result) {
                        var range = quill.getSelection(true);
                        quill.insertEmbed(range.index, 'image', result);
                        quill.setSelection(range.index + 1);
                    }
                };
                reader.readAsDataURL(file);
            };
        });
    }
};
