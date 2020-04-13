//$('#myModal').on('shown.bs.modal', function () {
//    $('#myInput').focus()
//})


//$(".custom-file-input").on("change", function () {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//});




$(document).ready(function () {
    $('#file-upload').change(function () {
        var i = $(this).prev('label').clone();
        var file = $('#file-upload')[0].files[0].name;
        //$(this).prev('#fileName').text(file);

        document.getElementById("fileName").innerHTML = file;
    });
    //lo nuevo
});

//funciones de jquery, javascript
 function ejemplo(){
    alert("ejemplo");
}