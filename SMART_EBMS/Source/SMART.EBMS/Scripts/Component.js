//查看产品信息
function ShowMatPartModel(MatID) {
    $('#MyMatPartModel').modal({ backdrop: 'static' });
    ReloadMatPart(MatID);
}

function CloseMatPartModel() {
    $('#MyMatPartModel').modal('hide');
}

function ReloadMatPart(MatID) {
    $("#MatPartContext").text("Loading...");
    $("#MatPartContext").load("/Component/Mat_Proview_Part/" + MatID)
}
//查看产品信息




//预览产品图片
function ShowMatImgModel(ImgPatch) {
    $('#MyMatImgModel').modal({ backdrop: 'static' });
    $("#MatImg").attr("src", ImgPatch);
}

function CloseMatImgModel() {
    $('#MyMatImgModel').modal('hide');
    $("#MatImg").attr("src", "");
}
//预览产品图片
