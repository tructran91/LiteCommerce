if (window.QuillFunctions) {
    var origLoadContent = window.QuillFunctions.loadQuillContent;
    window.QuillFunctions.loadQuillContent = function (quillElement, quillContent) {
        if (quillElement.__quill) {
            quillElement.__quill.clipboard.dangerouslyPasteHTML(quillContent);
        }
    };
}
