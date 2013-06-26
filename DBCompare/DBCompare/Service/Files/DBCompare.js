String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};
function toggleSection(opener) {
    var src = opener.attr('src');
    var state = src.endsWith('Down.png') ? 1 : 0;
    var newState = 1 - state;
    opener.attr('src', newState == 0 ? src.replace('Down.png', 'Right.png') : src.replace('Right.png', 'Down.png'));
    var details = opener.parents('.section').children('.details');
    details.slideToggle(100);
}
$(document).ready(function () {
    $('.section h2').each(function () {
        var content = $(this).html();
        $(this).html('<img src="/Files/Down.png" alt="open" class="imageButton icon16 opener" style="margin:0 10px 0 -10px" />' + content)
    })
    $('.opener').click(function () { toggleSection($(this)); });
});

function chooseColumns() {
    var columns = $('table.data th');
    var div = '<div class="columnChooser" title="Choose Columns"><ul>';
    for (var i = 0; i < columns.length; i++) {
        var visible = $('.' + columns[i].className).css('display') != 'none';
        div += '<li><input type="checkbox" id="' + columns[i].className + 'CheckBox" '
            + (visible ? 'checked="checked"' : '') + '/><label for="' + columns[i].className + 'CheckBox">' + columns[i].innerHTML + '</label></li>';
    }
    div += '</div></ul>';
    $(div).dialog({
        modal: true,
        buttons: [{ text: 'Close', click: function () { $(this).dialog('close'); } }]
    });
    $('.columnChooser input').change(function () { changeVisibility($(this)); });
}
function changeVisibility(checkBox) {
    var id = checkBox.attr('id');
    var className = id.substring(0, id.length - 'CheckBox'.length);
    if (checkBox[0].checked)
        $('.' + className).show();
    else
        $('.' + className).hide();
}