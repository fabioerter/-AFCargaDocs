﻿jQuery(document).ready(function () {
    $('#file-upload').change(function () {
        $('#ModalError').modal('hide');
        var i = $(this).prev('label').clone();
        var file = $('#file-upload')[0].files[0].name;
        document.getElementById("fileName").innerHTML = file;
    });

    $('#myModalCarga').on('hidden.bs.modal', function () {
        //debugger;
        document.getElementById("fileName").innerHTML = "";
        $('#file-upload').val('')
        $('#notFound').css("display", "none");
        //window.location.reload();
    });

    var docsalumn;
    ObtenerDocumentos();

    $('#trigger').click(function () {
        $("#dialog").dialog();
    });
    $('#top').css({
        'position': 'fixed',
        'right': '30px',
        'bottom': '30px',
    });
    $("#return").css({
        "position": "absolute",
        "right": "30px",
        "top": "30px",
    }).on("click", function () {
        window.history.back();
    });


});

//funciones de jquery, javascript
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

    try {

        let name = $('#file-upload')[0].files[0].name;

        if (name.length > 0) {

            var ruta;
            var lenght = $('#file-upload')[0].files[0].size;
            if (lenght / 1000 > 5000) {
                //$('#ModalError').modal('show');
                //return;
                throw Error("Este archivo excede el tamaño máximo permitido de 5 MB.");
            }

            fd = new FormData();
            fd.append('treqCode', document.getElementById("clave").innerHTML);
            $.ajax({
                url: 'CargaDocs/validaCarga',
                data: fd,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (data) {
                    if (JSON.parse(data).isOnServer) {
                        ruta = "CargaDocs/updateDocumento"
                    } else {
                        ruta = "CargaDocs/guardarDocumento"
                    }

                },
                error: function (errormessage) {
                    name = "";
                    $('#ModalError').modal('show');
                }
            });
            fd = new FormData();
            fd.append('file', $('#file-upload')[0].files[0]);
            fd.append('clave', document.getElementById("clave").innerHTML);// $('input[id="clave"]').val(clave)

            var mensaje = "quieres?"//$("#mi-modal").modal('show');
            //parte de mensaje ana
            $("#mi-modal").modal('show');

            $("#modal-btn-si").on("click", function () {
                $("#mi-modal").modal('hide');
                $.ajax({
                    url: ruta,
                    data: fd,
                    processData: false,
                    contentType: false,
                    type: "POST",
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
            });

            $("#modal-btn-no").on("click", function () {
                $("#mi-modal").modal('hide');
            });

            if (mensaje) {
            }
        }
    }
    catch (error) {
        $('#notFound').css("display", "block");
        //document.getElementById("#textoAdvertencia").innerHTML = "favor de seleccionar archivo";
        if (error.__proto__.name == "TypeError") {
            $("#textoAdvertencia").text("Favor de seleccionar un archivo");
        }
        else {
            $("#textoAdvertencia").text(error.message);
        }

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
            alert(errormessage.mensaje);
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

        if (fechas[7] == "1") {
            data[i].fecha = "";
        }
        //case para el boton de cargar document
        var chabit = "../images/Recursos/image15.png"
        var cdhabit = "../images/Recursos/image18.png"

        var comment = '';
        var cargadisabled = false;
        var estilocarga;

        switch (data[i].status) {//Status
            case "PS": //pendiente x subir
                CargaM = "../images/Recursos/image15.png";
                cargadisabled = true;
                estilocarga = "";
                break;
            case "NR"://Pendiente´x ser aprobados
                //menos de un mes se pode cargar el archivo novamiente
                if ((Date.now() - (((Date.parse(fechas)) ** 2) ** (1 / 2))) < 2628000000) {
                    CargaM = "../images/Recursos/image15.png";
                    cargadisabled = true;
                    estilocarga = "";
                }
                else {
                    CargaM = "../images/Recursos/image18.png";
                    cargadisabled = false;
                    estilocarga = "pointer-events: none;";
                }
                break;
            case "IV"://Rechazados
                CargaM = "../images/Recursos/image15.png";
                cargadisabled = true;
                estilocarga = "";
                comment = 'data-toggle="tooltip" data-placement="left" title="' + data[i].comment + '"';
                break;
            case "CM"://Validados y Aceptados
                CargaM = "../images/Recursos/image18.png";
                cargadisabled = false;
                estilocarga = "pointer-events: none;";
                break;
        }

        //Case para el boton de vista previa

        var vistadisabled = false;
        var estilovp;

        switch (data[i].status) {//Status
            case "PS"://Faltan de Subir
                VisP = "../images/Recursos/image14.png";

                vistadisabled = false;
                estilovp = "pointer-events: none;";
                break;
            case "NR"://Pendientes por ser aprobados
                VisP = "../images/Recursos/image13.png";
                vistadisabled = true;
                estilovp = "";
                break;
            case "IV"://Rechazados
                VisP = "../images/Recursos/image14.png";
                vistadisabled = false;
                estilovp = "pointer-events: none;";
                break;
            case "CM"://Validados y Aceptados
                VisP = "../images/Recursos/image13.png";
                vistadisabled = true;
                estilovp = "";
                break;
        }

        //Tabla con la lista de documentos

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
            '<input style="' + estilocarga + '"id="obtener" name="obtener" onclick="obtener(' + data[i].clave + ',' + cargadisabled + ')"  data-target="#myModalCarga" type=image src="' + CargaM + '">' +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div align="center">' +
            '<input style="' + estilovp + '" id="vistap" name="vistap" onclick="vistap(' + data[i].clave + ',' + vistadisabled + ')" type=image src="' + VisP + '">'//<input type=image src="' + VisP +'"> 
            +
            '</div>' +
            '</td>' +
            '<td>' +
            '' +
            '<div align="center">' +
            data[i].fecha +
            '</div>' +
            '</td>' +
            '<td>' +
            '<div align="center">' +
            '<img class="mainiconos" src="' + status + '"' + comment + '>' +
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




//accion del boton para carga documento
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
//accion del boton vista previa del documento
vistap = function (clave, vistadisabled) {
    var botonvistaprev = document.getElementById('vistap');

    if (vistadisabled) {
        var clave = clave.id;
        document.getElementById("clave").innerHTML = clave;
        window.open('/CargaDocs/FileDisplay/?treqCode=' + clave, '_blank', 'top=50,left=400,width=600px,height=650px,toolbar=1,resizable=0');
    }
    else {
    }
};


