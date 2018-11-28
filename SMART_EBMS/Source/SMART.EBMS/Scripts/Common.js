/**
* Common.js 1.0.0
* Copyright (c) 2012-2013 Richstar. All rights reserved.
* Author:Bill Shi
*/

$(function () {
    $.ajaxSetup({ cache: false });
    $("a").click(function () { LoadLineLoop(); });
    FixRightDivHeight();
    SetDataTableAutoWidth();
    setTimeout("Hide_Textarea_Error()", 100000);
});

function LoadLineLoop() {
    $('#MaskLoadLine').css({ width: '0%' });
    $('#MaskLoadLine').animate({ width: '100%' }, 500, CallBackAn);
}

var CallBackAn = function () {
    $('#MaskLoadLine').css({ width: '0%' });
};

$(window).resize(function () {
    FixRightDivHeight();
    SetDataTableAutoWidth();
});

function ReloadPage() {
    location.reload();
}

function Hide_Textarea_Error()
{
    $(".Textarea_Error").hide(200);
}

function Hide_Textarea_Success() {
    $(".Textarea_Success").hide(200);
}

function ActLeftMenu(Code) {
    $("#LeftMenu_Sub_" + Code).css("color", "white");
    $("#LeftMenu_Sub_" + Code).css("background-color", "#5c81be");
}

function TopMenuToAction(TopMenuID) {
    $("#MTB_" + TopMenuID).addClass("TopMenuAction");
}

function TopMenuSubToAction(IdName) {
    $("#TopSub_" + IdName).addClass("TopMenuNavSub_Sim-act");
}



function FixHeightValue() {
    //获取浏览器当前高度
    var WinHeight = $(window).height();

    //获取页面扣除DOM高度 - 重新获得列表框自适应高度
    var DeductionValue = 0;
    $('.DeductionDiv').each(function (i) {
        DeductionValue += $(this).outerHeight();
    })
    DeductionValue += $('.MyPageFoot').outerHeight();
    return WinHeight - DeductionValue;
}

function  FixHeightDataTable()
{
    return parseInt(30);
}

function FixHeightValue_DataTable() {
    return FixHeightValue() - 30;
}


function SetDataTableAutoWidth() {
    var Left_Menu_Width = $("#RightDiv").width();
    var MyWidth = $(window).width() - Left_Menu_Width;

    var style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = ".MySetWidth div.dataTables_wrapper {width: " + MyWidth + "px}";
    document.getElementsByTagName('HEAD').item(0).appendChild(style);
}

function FixRightDivHeight() {
    $('#RightDiv').css('height', FixHeightValue());
    $('#RightDivContext').css('height', FixHeightValue());
}

//表单验证按Class验证
function validateFormByClass(ClsName) {
    var AllSet = 0;

    $('.' + ClsName).each(function (i) {
        if ($(this).val() == "") {
            $(this).css("backgroundColor", "#D2E9FF");
            AllSet = AllSet + 1;
            obj = $(this);
        } else {
            $(this).css("backgroundColor", "#FFF");
        }
    })

    if (AllSet > 0) {
        $(obj).focus();
        return false;
    } else {
        return true;
    }
}

//获取复选框当前所选数
function checkBoxCheckLength(classStr) {
    var Loop = 0;
    Loop = $("." + classStr + ":checked").length;
    return Loop;
}

//获取全选操作
function checkBoxCheckAll(checkID, classStr) {

    var isCheck = document.getElementById(checkID).checked
    if (isCheck == true) {
        $('.' + classStr).each(function (i) {
            $(this).prop({ checked: true });
        })
    } else {
        $('.' + classStr).each(function (i) {
            $(this).prop({ checked: false });
        })
    }
}

function DisAndEnabledByID(ID, Val) {
    if (Val == 1) {
        $('#' + ID).attr("disabled", true);
    } else {
        $('#' + ID).attr("disabled", false);
    }
}

function DisAndEnabledByClass(CLS, Val) {
    if (Val == 1) {
        $('.' + CLS).attr("disabled", true);
    } else {
        $('.' + CLS).attr("disabled", false);
    }
}

function DisAndEnabledBtn(Val) {
    if (Val == 1) {
        $(':button').attr("disabled", true);
    } else {
        $(':button').attr("disabled", false);
    }
}


//AjaxForm
function AjaxFormPost(FormObj) {
    var PostData = $(FormObj).serialize();
    var Url = $(FormObj).prop("action");
    var IsError = false;

    $.ajax({
        type: "post",
        url: Url,
        async: false,
        cache: false,
        dataType: "json",
        data: PostData,
        success: function (Jsondata) {
            if (Jsondata.ErrorMessage != "") {
                alert(Jsondata.ErrorMessage);
                IsError = true;
            }

            if (Jsondata.SuccessMessage != "") {
                alert(Jsondata.SuccessMessage);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            IsError = true;
            alert(errorThrown);
        }
    });

    return IsError;
}

function ShowHelp(ConAct)
{
    $(".SubTopMenuDivHelpSub").toggle(200);
    $("#SubTopMenuDivHelpContext").text("Loading...");
    $("#SubTopMenuDivHelpContext").load("/Help/HelpContext?ConAct=" + ConAct);
}

function HideHelp()
{
    $(".SubTopMenuDivHelpSub").hide(200);
}

function ShowAnswer(Obj) {
    $(".TopHelpBox").show();
    $(".TopHelpBox p").hide();
    $(Obj).next().show(100);
}


