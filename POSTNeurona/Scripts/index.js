
let dataSet = JSON.parse(localStorage.getItem('dataSet')) || [];
//TEXTAREA Readonly
$('#responseTextarea').attr('readonly', 'readonly');
// add the rule here
$.validator.addMethod("valueNotEquals", function (value, element, arg) {
    return arg !== value;
}, "El campo es obligatorio");
//Init form validate
$("#myForm").validate({
    errorElement: 'div',
    rules: {
       
        // compound rule
        url_input: {
            required: true
            //url: true
        },
        method_api: {
            required: true
        },
        sub_metodo: {
            required: true
        },
        method_input: {
            valueNotEquals: "default"
        }
    },
    messages: {
        
        url_input: {
            required: "El campo es obligatorio"
            //url: "La URL es incorrecta"
        },
        method_api: {
            required: "El campo es obligatorio"
        },
        sub_metodo: {
            required: "El campo es obligatorio"
        },
        method_input: {
            valueNotEquals: "El campo es obligatorio"
        }
    },
    submitHandler: function (form) {
        loadingValue();
        $.ajax({
            contentType: "application/json",
            url: '/api/HttpServiceNeurona',
            type: "POST",
            data: JSON.stringify({
                "url": $("#url_input").val(),
                "methodApi": $('#method_api option:selected').val(),
                "subMethodApi": $('#sub_metodo option:selected').val(), 
                "method": $('#method_input option:selected').val(),
                "body": $('textarea#requestTextarea').val()
            }),
            processData: false,
            async: true,
            cache: false,
            success: function (objJson) {
                const { OK, msg, id } = objJson;
                
                initValue();
                setTextareaValue(msg);
                if (OK) {
                    
                    dataSet.push(
                        {
                            "id": id,
                            "url": $("#url_input").val(),
                            "methodApi": $('#method_api option:selected').val(),
                            "subMethodApi": $('#sub_metodo option:selected').val(), 
                            "method": $('#method_input option:selected').val(),
                            "body": $('textarea#requestTextarea').val(),
                            "response": msg
                        }
                    );
                    localStorage.setItem('dataSet', JSON.stringify(dataSet));           
                    addHistorial({
                        "id": id,
                        "url": $("#url_input").val(),
                        "methodApi": $('#method_api option:selected').val(),
                        "subMethodApi": $('#sub_metodo option:selected').val(), 
                        "method": $('#method_input option:selected').val(),
                        "body": $('textarea#requestTextarea').val()
                    }, true
                    );
                }
                
                //setTextareaValue(objJson);
            },
            error: function (err) {
                const { responseJSON } = err;

                initValue();
                //disableFormat();
                enableFormat();
                setTextareaValue(JSON.stringify(responseJSON));

            }
        })

        return false;
    }
});

//Set TEXTAREA
const setTextareaValue = (value) => {
    
    $("#responseTextarea").val(
        value
    );
    $("#responseTextarea").focus();
}

// Disable format
const disableFormat = () => {
    $("#jsonFormat").prop("disabled", true);
    $("#xmlFormat").prop("disabled", true);
    $("#copyResponse").prop("disabled", true);
    $("#clearResponse").prop("disabled", true);
    $("#exportJson").prop("disabled", true);
    $("#exportXml").prop("disabled", true);
}
disableFormat();
// Enable format
const enableFormat = () => {
    $("#jsonFormat").prop("disabled", false);
    $("#xmlFormat").prop("disabled", false);
    $("#copyResponse").prop("disabled", false);
    $("#clearResponse").prop("disabled", false);
    $("#exportJson").prop("disabled", false);
    $("#exportXml").prop("disabled", false);
}

// Loading STATE (Disable buttom and Change Text)
const loadingValue = () => {
    $("#buttom_text").text('Cargando...')
    $("#sendReuqest").prop("disabled", true);
    $("#copyResponse").prop("disabled", true);
    $("#clearResponse").prop("disabled", true);
    $("#jsonFormat").prop("disabled", true);
    $("#xmlFormat").prop("disabled", true);
    $("#exportJson").prop("disabled", true);
    $("#exportXml").prop("disabled", true);
    const getMethondRequest = $('#method_input option:selected').val();
    if (getMethondRequest == "2" || getMethondRequest == "3") {
        $("#jsonFormatRequest").prop("disabled", true);
        $("#clearRequest").prop("disabled", true);
    }
}

// Init STATE  (Enable buttom and Change Text)
const initValue = () => {
    $("#buttom_text").text('Aceptar')
    $("#sendReuqest").prop("disabled", false);
    $("#copyResponse").prop("disabled", false);
    $("#clearResponse").prop("disabled", false);
    $("#jsonFormat").prop("disabled", false);
    $("#xmlFormat").prop("disabled", false);
    $("#exportJson").prop("disabled", false);
    $("#exportXml").prop("disabled", false);
    const getMethondRequest = $('#method_input option:selected').val();
    if (getMethondRequest == "2" || getMethondRequest == "3") {
        $("#jsonFormatRequest").prop("disabled", false);
        $("#clearRequest").prop("disabled", false);
    }
}

//Clear Text area
$('#clearResponse').click(function () {
    $("#responseTextarea").val("")
});

// Copy Clipboard (Textarea)
$("#copyResponse").click(function (e) {
    e.preventDefault();
    if ($("#responseTextarea").val() != '') {
        document.execCommand('copy', false, document.getElementById('responseTextarea').select());
    }
});

//JSON Format (Response)
$("#jsonFormat").click(function (e) {
    e.preventDefault();
    try {
        $('#responseTextarea').format({ method: 'json' });
    } catch (error) {
        Swal.fire({
            title: 'Error!',
            text: `Error JSON Formatter ${error.message}`,
            icon: 'error',
            confirmButtonText: 'OK'
        }).then(() => {
            //$("#responseTextarea").val("")
            $("#responseTextarea").focus();
        });

    }
    
});
//XML Format (Response)
$("#xmlFormat").click(function (e) {
    e.preventDefault();
    try {
        $('#responseTextarea').format({ method: 'xml' });
    } catch (error) {
        
        Swal.fire({
            title: 'Error!',
            text: `Error XML Formatter ${error.message}`,
            icon: 'error',
            confirmButtonText: 'OK',
            confirmButtonColor: '#F05454',
        }).then(() => {
            //$("#responseTextarea").val("")
            $("#responseTextarea").focus();
        });

    }
    
});

//Exportar JSON
$("#exportJson").click(function () {
    var fechaConcat = fecConcat();
    var blob = new Blob([$("#responseTextarea").val()], { type: "text/json;charset=utf-8" });
    saveAs(blob, fechaConcat + ".json");
});

//Exportar XML
$("#exportXml").click(function () {
    var fechaConcat = fecConcat();

    //Este es un ejemplo
    var algo = "<Envelope xmlns='http://schemas.xmlsoap.org/soap/envelope/'><Body><emitirDocumentosSoapV2 xmlns='https://www-prueba.titanio.com.co/PDE/public/api/soap/metodos'><token xmlns=''>[string]</token><tr_tipo_id xmlns=''>[int]</tr_tipo_id><data xmlns=''>[string]</data></emitirDocumentosSoapV2 ></Body ></Envelope >";
    var blob = new Blob([algo], { type: "text/xml;charset=utf-8" });
    saveAs(blob, fechaConcat + ".xml");
});

function fecConcat() {
    let fecha = new Date();
    let dia = fecha.getDay();
    let mes = fecha.getMonth() + 1;
    let annio = fecha.getFullYear();
    let sec = fecha.getSeconds();
    let fullFec = dia +""+ mes +""+ annio +""+ sec;
    return fullFec;
}

//Toogle BodyTextarea (Request)
$("#requestTextareaContainer").hide();
$('#method_input').on('change', function () {
    const value = this.value;
    $("#requestTextarea").val("");
    if (value == '2' || value == '3') {
        
        $("#requestTextareaContainer").show();
    } else {
        $("#requestTextareaContainer").hide();
    }
});

//JSON Format (Request)
$("#jsonFormatRequest").click(function (e) {
    e.preventDefault();
    try {
        $('#requestTextarea').format({ method: 'json' });
    } catch (error) {
        
        Swal.fire({
            title: 'Error!',
            text: `Error JSON Formatter ${error.message}`,
            icon: 'error',
            confirmButtonColor: '#F05454',
            confirmButtonText: 'OK'
        }).then(() => {
            //$("#requestTextarea").val("")
            $("#requestTextarea").focus();
        });
       
    }

   
});

//Clear Request Text area 
$('#clearRequest').click(function () {
    $("#requestTextarea").val("")
});


//Truncate URL (Https://google.com/...)
function truncate(str, n) {
    return (str.length > n) ? str.substr(0, n - 1) + '&hellip;' : str;
};
//set listaHistorial (Add HTML into Cointarer of Historial)
const setListaHistorial = ({ id, method, url }, active = false) => {
    let estilo = "";
    let metodo = "";

    switch (method) {
        case "1":
            estilo = "text-success";
            metodo = "GET";
            break;
        case "2":
            estilo = "text-warning";
            metodo = "POST";
            break;
        case "3":
            estilo = "text-info";
            metodo = "PUT"
            break;
    }
    
    const content = `<div class="list-group-item list-group-item-action list-item-container ${active&& "active"}" id="${id}">
                     <div class="d-flex w-100 justify-content-between">
                     <h5 class="mb-1 fw-bold ${estilo}">${metodo}</h5>
                     <button type="button" class="btn-close btn-close-white close-item" id="close_${id}"></button>
                     </div>
                     <p class="mb-1 text-decoration-underline ">${truncate(url, 45)}</p>
                     </div> `;
    $('#listaHistorial:first').prepend(content);
}
//SET List Group (Add all items on data set into list-group)
const setHistorial = (dataSet) => {
    const data = dataSet;
    $('#listaHistorial').html("");
    if (dataSet.length > 0) {
        
        data.forEach(({ id, url, method, body }) => {
            setListaHistorial({ id, method, url })
        });
    }
    
    
    
}
//SET List group (Init historial)
setHistorial(dataSet);

//Add Item (one) to historial
const addHistorial = ({ id, method, url, body,msg }, active = false) => {

    $(".list-item-container").removeClass("active");
    localStorage.setItem('dataSet', JSON.stringify(dataSet));
    setListaHistorial({ id, method, url }, active)
    
}


//Click set 1 item (ListItem) on inputs
$('#listaHistorial').on('click', '.list-item-container',function () {
    var id_container = $(this).attr('id');
   
    var item = dataSet.filter( ({ id}) =>
        ( id === id_container)
    );

    if (item.length > 0) {
        const data = item[0]
        $("#url_input").val(data.url);
        $("#method_api").val(data.methodApi).change();
        $("#method_input").val(data.method).change();
        $('textarea#requestTextarea').val(data.body);
        setTextareaValue(data.response);
        $("#" + id_container).addClass('active').siblings('.active').removeClass('active');
    }
    
});

//Clear dataset localstorage y listview (Clear all item of list-group)
$('#clear-items').click(function () {
    localStorage.clear();
    dataSet = [];
    setHistorial(dataSet);
    return false;
});

//Clear one item of list group
$('#listaHistorial').on('click', '.btn-close',function () {
    var id_container = $(this).attr('id').split('_')[1];
   
    var items = dataSet.filter(({ id }) =>(id !== id_container) );
    dataSet = items;
    localStorage.clear();
    localStorage.setItem('dataSet', JSON.stringify(items));  
    
    setHistorial(items);
    

       return false;
});

function cargarMetodos() {
    var array = ["Ahorro", "Blindaje Total", "Ciberseguridad", "Fenix", "GMM", "Hogar Protegido", "No Autos","Middleware"];
    array.sort();
    addOptions("method_api", array);
}

//Función para agregar opciones a un <select>.
function addOptions(domElement, array) {
    var selector = document.getElementsByName(domElement)[0];
    var num = 1;
    for (metodo in array) {
        var opcion = document.createElement("option");
        opcion.text = array[metodo];
        // Añadimos un value a los option para hacer mas facil escoger los pueblos
        opcion.value = num;
        num++;
        selector.add(opcion);
    }
}

function cargarSubMetodos() {
    // Objeto de metodos con subMetodos
    var subMetodos = {
        3: ["Documentos/Endosos", "Cotizaciones", "Cumulos/Negative File", "Catalogos", "RFC"]
    }

    var metodos_api = document.getElementById('method_api')
    var submetodos_api = document.getElementById('sub_metodo')
    var MetodoSeleccionado = metodos_api.value

    // Se limpian los sub metodos api
    submetodos_api.innerHTML = '<option value="">Seleccione...</option>'

    if (MetodoSeleccionado !== '') {
        // Se seleccionan los sub metodos y se ordenan
        MetodoSeleccionado = subMetodos[MetodoSeleccionado]
        if (MetodoSeleccionado !== undefined) {
            document.getElementById('sub_metodo').disabled = false;
            MetodoSeleccionado.sort()
            MetodoSeleccionado.forEach(function (submetodosApi, index) {
                let opcion = document.createElement('option')
                opcion.value = index
                opcion.text = submetodosApi
                submetodos_api.add(opcion)
            });
        } else {
            document.getElementById('sub_metodo').disabled = true;
        }
    } 
}

// Iniciar la carga de Metodos
cargarMetodos();