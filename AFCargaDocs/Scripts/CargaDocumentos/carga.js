//$('#myModal').on('shown.bs.modal', function () {
//    $('#myInput').focus()
//})


//$(".custom-file-input").on("change", function () {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//});

jQuery(document).ready(function () {
    $('#file-upload').change(function () {
        $('#ModalError').modal('hide');
        var i = $(this).prev('label').clone();
        var file = $('#file-upload')[0].files[0].name;
        //$(this).prev('#fileName').text(file);
        document.getElementById("fileName").innerHTML = file;
        //if (file.)
        guardar();
        //$("#SubirArchivo").prop("disabled", false);
        //$("notFound").css()

        
    });

    $('#myModalCarga').on('hidden.bs.modal', function () {
        document.getElementById("fileName").innerHTML = "";
        $('#notFound').css("display", "none");
        //document.getElementById("#textoAdvertencia").innerHTML = "";
    });
    //lo nuevo
});

//funciones de jquery, javascript
 function ejemplo(){
    alert("ejemplo");
}


function escapeTags(str) {
    return String(str)
        .replace(/&/g, '& amp; ' )
.replace(/"/g, '& quot; ' )
.replace(/</g, '& lt; ' ) .replace(/>/g, '& gt; ' );
}

function guardar()
{
        try
        {

        let name = $('#file-upload')[0].files[0].name;
            if (name.length > 0) {

                var fd = new FormData();
                fd.append('file', $('#file-upload')[0].files[0]);
                fd.append('clave', 'FIRMA')
                //Ingresamos un mensaje a mostrar
                var mensaje = confirm("Acepta cargar el archivo?");
                //Detectamos si el usuario acepto el mensaje
                if (mensaje) {
                    $.ajax({
                        url: 'CargaDocs/guardarDocumento',
                        data: fd,
                        processData: false,
                        contentType: false,
                        type: 'POST',
                        success: function (data) {
                            document.getElementById("fileName").innerHTML = "";
                            name = "";
                            $('#myModalCarga').modal('hide');
                            $('#ModalExito').modal('show');

                        },
                        error: function (errormessage) {
                            name = "";
                            $('#ModalError').modal('show');
                        }
                    });
                }
            }
        }
    catch (error)
            {
            $('#notFound').css("display", "block");
            //document.getElementById("#textoAdvertencia").innerHTML = "favor de seleccionar archivo";
            $("#textoAdvertencia").text("favor de seleccionar archivo");
            }  
}



window.onload = function () {


    
};