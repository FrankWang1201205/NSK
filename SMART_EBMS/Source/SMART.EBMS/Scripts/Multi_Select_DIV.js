//多选框
function Show_Multi_Select_DIV(Obj) {

    var Multi_Select_Obj = $(Obj).parent().parent().parent().parent().parent();

    var Obj_DIV = $(Multi_Select_Obj).find(".Multi_Select_DIV:first");
    $(Obj_DIV).toggle();
    $(".Multi_Select_DIV").not(Obj_DIV).hide();

    var Obj_DIV_Parent = $(Obj_DIV).parent();
    var P_Top = $(Obj_DIV_Parent).offset().top;
    var P_Height = $(Obj_DIV_Parent).outerHeight() - 1;
    $(Obj_DIV).css('top', P_Top + P_Height);
}

function Get_Multi_Select_DIV_Check_Val(Obj)
{
    var Check_Box_List = $(Obj).find(":checkbox");
    var Check_Box_Val = "";
    $(Check_Box_List).each(function (i) {
        if ($(this).prop('checked') == true) {
            Check_Box_Val += $(this).val()+",";
        }
    });

    Check_Box_Val = Check_Box_Val.substring(0, Check_Box_Val.length - 1);
    var Obj_Parent_Text = $(Obj).parent().find(":text:first");
    $(Obj_Parent_Text).val(Check_Box_Val);
}



$(".Multi_Select_DIV").hover(function () {
    $(this).show();
}, function () {
    $(this).hide();
    Get_Multi_Select_DIV_Check_Val($(this));
});