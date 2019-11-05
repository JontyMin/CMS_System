


//禁止一切弹出窗口、对话框、消息框进行移动操作
(function ($) {
    var __onOpen = function () {
        setTimeout(function () {
            $("div.window div.panel-title").unbind("mousedown").unbind("mousemove");
        }, 100);
    }
    $.fn.panel.defaults.onOpen = __onOpen;
    $.fn.window.defaults.onOpen = __onOpen;
    $.fn.dialog.defaults.onOpen = __onOpen;

    var _show = jQuery.messager.show;
    var _alert = jQuery.messager.alert;
    var _confirm = jQuery.messager.confirm;
    var _prompt = jQuery.messager.prompt;
    var _progress = jQuery.messager.progress;
    $.messager.show = function () { _show.apply($.messager, arguments); __onOpen(); }
    $.messager.alert = function () { _alert.apply($.messager, arguments); __onOpen(); }
    $.messager.confirm = function () { _confirm.apply($.messager, arguments); __onOpen(); }
    $.messager.prompt = function () { _prompt.apply($.messager, arguments); __onOpen(); }
    $.messager.progress = function () { _progress.apply($.messager, arguments); __onOpen(); }
})(jQuery);

