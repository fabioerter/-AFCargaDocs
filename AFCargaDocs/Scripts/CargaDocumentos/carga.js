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
        
        //$("#SubirArchivo").prop("disabled", false);
        //$("notFound").css()


    });

    $('#myModalCarga').on('hidden.bs.modal', function () {
        //debugger;
        document.getElementById("fileName").innerHTML = "";
        $('#notFound').css("display", "none");
        //document.getElementById("#textoAdvertencia").innerHTML = "";
    });

    var docsalumn;
    ObtenerDocumentos();

    $('#trigger').click(function () {
        $("#dialog").dialog();
    });
    //lo nuevo
});

//funciones de jquery, javascript
function ejemplo() {
    //alert("ejemplo");
}
function refrescar() {
    window.location.reload();
}

function escapeTags(str) {
    return String(str)
        .replace(/&/g, '& amp; ')
        .replace(/"/g, '& quot; ')
        .replace(/</g, '& lt; ').replace(/>/g, '& gt; ');
}
//metodo para guardar el archivo
function borrar() {
    $('#notFound').css("display", "none");
}
function guardar() {
    //var contactoObj = {
    //    TREQ_CODE: "AFCD",
    //    DOCUMENTO: "nombredoc.pdf"
    //};
 
    try {

        let name = $('#file-upload')[0].files[0].name;
 
        if (name.length > 0) {

            var fd = new FormData();
            fd.append('file', $('#file-upload')[0].files[0]);
            fd.append('clave', document.getElementById("clave").innerHTML);// $('input[id="clave"]').val(clave)
            //fd.append('clavee', texto);
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
    catch (error) {
        $('#notFound').css("display", "block");
        //document.getElementById("#textoAdvertencia").innerHTML = "favor de seleccionar archivo";
        $("#textoAdvertencia").text("Favor de seleccionar un archivo");
    }
}

//metodo para obtener la lista de documentos
function ObtenerDocumentos() {
    $.ajax({
        url: 'cargaDocs/ObtenerDocumentos',
        type: 'POST',
        dataType: "json",
        success: function (data) {
            docsalumn = data;

            createRows(data);
            //Clave: "AFCD"
            //Name: "Comprobante de domicilio"
            //Status: "PS"
            //Fecha: "18/02/2020"
        },
        error: function (errormessage) {
            alert("fallo");
        }
    });
};

//funcion para construir la informacion
function createRows(data) {


    //[{ "Clave": "AFCD", "Name": "Comprobante de domicilio", "Status": "PS", "Fecha": "18/02/2020" }, { "Clave": "FIRMA", "Name": "Solicitud llena y firmada", "Status": "PS", "Fecha": "18/02/2020" }, { "Clave": "TSAF", "Name": "Comprobante de pago de Solicitud de Ayuda Financiera", "Status": "PS", "Fecha": "18/02/2020" }]
    var rows = "";
    var status = "";
    var mes = "";
    var VisP = "";
    var CargaM = ""

    //Las variables de los estatus del documento
    var PS = 0;
    var NR = 0;
    var IV = 0;
    var CM = 0;


    for (var i = 0; i < data.length; i++) {
        //se les asigna una imagen segun case
        switch (data[i].status) {
            case "PS": //pendiente x subir
                status = "../images/Recursos/image9.png";
                PS++;
                break;
            case "NR"://Pendiente´x ser aprobados
                status = "../images/Recursos/image10.png";
                NR++;
                break;
            case "IV"://Rechazados
                status = "../images/Recursos/image12.png";
                IV++;
                break;
            case "CM"://Completo
                status = "../images/Recursos/image11.png";
                CM++;
                break;
            //default:

        }

        fechas = data[i].fecha;

        ////case para el mes en letras
        //switch (fechas[1]) {
        //    case "01":
        //        mes = "ENE";
        //        break;
        //    case "02":
        //        mes = "FEB";
        //        break;
        //    case "03":
        //        mes = "MAR";
        //        break;
        //    case "04":
        //        mes = "ABR";
        //        break;
        //    case "05":
        //        mes = "MAY";
        //        break;
        //    case "06":
        //        mes = "JUN";
        //        break;
        //    case "07":
        //        mes = "JUN";
        //        break;
        //    case "08":
        //        mes = "JUL";
        //        break;
        //    case "09":
        //        mes = "AGO";
        //        break;
        //    case "10":
        //        mes = "SEP";
        //        break;
        //    case "11":
        //        mes = "OCT";
        //        break;
        //    case "12":
        //        mes = "NOV";
        //        break;
        //    case "13":
        //        mes = "DIC";
        //        break;
        //    //default:
        //    //    mes = "";
        //}
        //case para el boton de cargar document
        var chabit = "../images/Recursos/image15.png"
        var cdhabit = "../images/Recursos/image18.png"

        var cargadisabled = false;
        switch (data[i].status) {//Status
            case "PS": //pendiente x subir
                CargaM = "../images/Recursos/image15.png";
                cargadisabled = true;
                break;
            case "NR"://Pendiente´x ser aprobados
                CargaM = "../images/Recursos/image18.png";
                cargadisabled = false;
                break;
            case "IV"://Rechazados
                CargaM = "../images/Recursos/image15.png";
                cargadisabled = true;
                break;
            case "CM"://Validados y Aceptados
                CargaM = "../images/Recursos/image18.png";
                cargadisabled = false;
                break;
        }

        //Case para el boton de vista previa

        var vistadisabled = false;
        switch (data[i].status) {//Status
            case "PS"://Faltan de Subir
                VisP = "../images/Recursos/image14.png";
                vistadisabled = false;
                break;
            case "NR"://Pendientes por ser aprobados
                VisP = "../images/Recursos/image13.png";
                vistadisabled = true;
                break;
            case "IV"://Rechazados
                VisP = "../images/Recursos/image14.png";
                vistadisabled = false;
                break;
            case "CM"://Validados y Aceptados
                VisP = "../images/Recursos/image13.png";
                vistadisabled = true;
                break;
        }

        //trabajar con la validacion de los casos

        rows += '<tr name="clave" id="' + data[i].clave + '">' +
            '<td style="width:11%">' +
            '<div style="display:flex;flex-direction:row;justify-content:flex-start;align-items:center">' +
            '<div>' +
            '<img class="mainiconos" src="../images/Recursos/image16.png" />' +
            '</div>&nbsp' +
            '<div style=padding: 20px;">' +
            data[i].name +
            '' +
            '</div>' +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div align="center">' +
            '<input onclick="obtener(' + data[i].clave + ',' + cargadisabled + ')"  data-target="#myModalCarga" type=image src="' + CargaM + '">' +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div align="center">' +
            '<input  target="_blank" onclick="vistap(' + data[i].clave + ',' + vistadisabled + ')" type=image src="' + VisP + '">'//<input type=image src="' + VisP +'"> 
            +
            '</div>' +
            '</td>' +
            '<td>' +
            '' +
            '<div align="center">' +
            fechas +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div align="center">' +
            '<img class="mainiconos" src="' + status + '">' +
            '</div>' +
            '</td>' +
            '</tr>';
    }
    $("#tbodydocsalumn").append(rows);
    $("#PS").append(PS);
    $("#NR").append(NR);
    $("#IV").append(IV);
    $("#CM").append(CM);


};

obtener = function (clave, cargadisabled) {

    if (cargadisabled) {
        $('#myModalCarga').modal('show');
        var clave = clave.id
        document.getElementById("clave").innerHTML = clave;
        $('input[name="clave"]').val(clave);
       
    }
    else {
        $('#myModalCarga').modal('hide');
    }


};

vistap = function (clave, vistadisabled) {
    if (vistadisabled) {
        var clave = clave.id;//document.getElementById("clave").innerHTML
        document.getElementById("clave").innerHTML = clave;

        //$.ajax({
        //    url: 'CargaDocs/FileDisplay',
        //    data: { "treqCode": clave },
        //    datatype: 'json',
        //    type: 'POST',
        //    success: function (data) {
        //        //$('#ModalExito').modal('show');
        //        $("#modal_contrato .modal-body").html(data);
        //    },
        //    error: function (errormessage) {
        //        name = "";
        //        $('#ModalError').modal('show');
        //    }
        //});
        window.open('/CargaDocs/FileDisplay/?treqCode=' + clave, '_blank', 'top=100,left=400,width=600px,height=500px,toolbar=1,resizable=0');
        //window.open("http://localhost:57705/CargaDocs/FileDisplay/?treqCode=" + clave, "_blank", "top=100,left=400,width=600px,height=500px");
    }
};
