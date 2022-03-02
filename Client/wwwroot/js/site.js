
$(document).ready(function () {
    $('#datatableSW').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "ajax": {
            "url": "https://localhost:44331/API/Employees/registeredData",
            "dataType": "json",
            "dataSrc": "result"
        },
        "columns": [
            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": "fullName"
            },
            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return `${phoneNumber(row['phone'])}`;
                }
            },
            {
                "data": null,
                render: function (data, type, row) {
                    return `Rp. ${addCommas(row['salary'])}`
                }   
            },
            {
                "data": null,
                render: function (data, type, row) {
                    return `${row['birthdate'].substring(0, 10)}`
                }
            },
            {
                "data": null,
                "targets": 0,
                "orderable": false,
                render: function (data, type, row) {
                    return `
                       <button type="button" class="btn btn-sm border-dark px-3" data-toggle="modal" data-target="#modalDetail" onclick="detailEmployee('${row['nik']}')"><i class="fa fa-info text-dark"></i></button>
                       <button type="button" class="btn btn-sm border-success" data-toggle="modal" data-target="" onclick="showModalEdit('${row['nik']}')"><i class="fa fa-edit text-success"></i></button>
                       <button type="button" class="btn btn-sm border-danger"><i class="fa fa-trash text-danger" onclick="deleteEmployee('${row['nik']}')"></i></button>
                   `;
                }
            }
        ], 
    });
});


// load university
$(document).ready(function () {

    loadUniversity('#universityInput');
 /*
    $('#universityInput').change(function () {
        alert('yussan');
    });*/

    function loadUniversity(id) {
      
        $.ajax({
            url: 'https://localhost:44331/API/Universities',
            // dataType: 'json',
            success: function (response) {
                $(id).html('<option>Pilih</option>');
                $university = '';
               
                response.result.forEach(univ => {
                    university = `<option class="text-capitalize" value="${univ.id}">${univ.name}</option>`;
                    university = university + '';
                    $(id).append(university);
                });
            },
            error: function () {
                $(id).html('ERROR');
            }
        });
    }
})


// load degree
$(document).ready(function () {

    loadDegree('#degreeInput');
    /*
       $('#universityInput').change(function () {
           alert('yussan');
       });*/

    function loadDegree(id) {
        $.ajax({
            url: "https://localhost:44331/API/Educations/degree",
            success: function (response) {
                $(id).html('<option>Pilih</option>');
                $degree = '';

                response.forEach(deg => {
                    degree = `<option value="${deg.name}">${deg.name}</option>`;
                    degree = degree + '';
                    $(id).append(degree);
                });
            },
            error: function () {
                $(id).html('ERROR');
            }
        });
    }
})


// load gender
$(document).ready(function () {

    loadGender('#genderInput');

    function loadGender(id) {
        $.ajax({
            url: "https://localhost:44331/API/Employees/gender",
            success: function (response) {
                $(id).html('<option>Pilih</option>');
                $gender = '';

                response.forEach(gen => {
                    gender = `<option value="${gen.gender}">${getGender(gen.gender)}</option>`;
                    gender = gender + '';
                    $(id).append(gender);
                });
            },
            error: function () {
                $(id).html('ERROR');
            }
        });
    }
})


$(document).ready(function () {
    generateNIK("#inputNIK")
    
    function generateNIK(id) {
        $.ajax({
            url: "https://localhost:44331/API/Employees/generateNik",
            success: function (result) {
                $(id).html(`${result.value}`);
                console.log(`nik anda : ${result.value}`);
            }
        });
    }
})



function createEmployee() {
    var obj = new Object();

    obj.FirstName = $("#firstNameInput").val();
    obj.LastName = $("#lastNameInput").val();
    obj.Phone = $("#phoneInput").val();
    obj.BirthDate = $("#dateInput").val();
    obj.Salary = $("#salaryInput").val();
    obj.Email = $("#emailInput").val();
    obj.Gender = $("#genderInput").val();
    obj.Password = $("#passwordInput").val();
    obj.Degree = $("#degreeInput").val();
    obj.GPA = $("#gpaInput").val();
    obj.University_Id = $("#universityInput").val();
    console.log(obj);

    $.ajax({
        url: "https://localhost:44331/API/Employees/Register",
        type: "POST",
        headers: {
            'Authorization': '(redacted)',
            'Access-Control-Allow-Origin': '*',
            'x-giftup-testmode': true
        },
        crossDomain: true,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType:"json",
    }).done((result) => {
        alert("insert is succesfully");
        clearInputBox();
        $('#modalCreate').modal("hide");
        $('#datatableSW').DataTable().ajax.reload();
    }).fail((error) => {
        alert("insert is fail");
    })
}


function showModalEdit(nik) {
    $.ajax({
        url: "https://localhost:44331/API/Employees/" + nik,
        success: function (data) {
            console.log(data);
            $('#modalEdit').modal("show");

            $("#nikEdit").val(data.result.nik);
            $("#firstNameEdit").val(data.result.firstName);
            $("#lastNameEdit").val(data.result.lastName);
            $("#phoneEdit").val(data.result.phone);

            var date = data.result.birthDate.substring(0, 10);
            $("#dateEdit").val(date);

            $("#salaryEdit").val(data.result.salary);
            $("#emailEdit").val(data.result.email);
            $("#genderEdit").val(data.result.gender);
            
        }
    })
}


function editEmployee() {
    var obj = new Object();

    obj.nik = $("#nikEdit").val();
    obj.firstName = $("#firstNameEdit").val();
    obj.lastName = $("#lastNameEdit").val();
    obj.phone = $("#phoneEdit").val();
    obj.birthDate = $("#dateEdit").val();
    obj.salary = $("#salaryEdit").val();
    obj.email = $("#emailEdit").val();
    obj.gender = $("#genderEdit").val();
  
    console.log(obj);
    $.ajax({
        url: "https://localhost:44331/API/Employees",
        type: "PUT",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        }).done((result) => {
            alert("update is succesfully");
            $('#modalEdit').modal("hide");
            $('#datatableSW').DataTable().ajax.reload();
        }).fail((error) => {
            alert("update is fail");
    });
}

function detailEmployee(nik) {
    contentModal = "";
    $.ajax({
        url: "https://localhost:44331/API/Employees/" + nik,
        success: function (data) {
            contentModal += `
                    <div class="row">
                        <div class="col-md">
                            <table class="table table-sm">
                                  <tbody>
                                     <tr>
                                        <th>Nik</th>
                                        <td>: ${data.result.nik}</td>
                                     </tr>
                                     <tr>
                                        <th>Full Name</th>
                                        <td class="text-capitalize">: ${data.result.firstName} ${data.result.lastName}</td>
                                     </tr>
                                     <tr>
                                        <th>Email</th>
                                        <td>: ${data.result.email}</td>
                                     </tr
                                     <tr>
                                        <th>Phone</th>
                                        <td>: ${phoneNumber(data.result.phone)}</td>
                                     </tr>
                                     <tr>
                                        <th>Gender</th>
                                        <td>:${getGender(data.result.gender)}</td>
                                     </tr>
                                     <tr>
                                        <th>Nik</th>
                                        <td>: ${data.result.birthDate.substring(0, 10)}</td>
                                     </tr>
                                     <tr>
                                        <th>Salary</th>
                                        <td>: Rp. ${addCommas(data.result.salary)}</td>
                                     </tr>
                                  </tbody>
                             </table>
                          </div>
                      </div>
                   </div>
                 </div>
               `
            $('.contentData').html(contentModal);
        }
    })
}


function deleteEmployee(nik) {
    var confirmation = confirm("are you sure want to delete the item ?");
    if (confirmation) {
        $.ajax({
            url: "https://localhost:44331/API/Employees/" + nik,
            type: "DELETE",
        }).done((result) => {
            alert('item has been deleted');
            $('#datatableSW').DataTable().ajax.reload();
        }).fail((error) => {
            alert("delete is fail");
        });
    }
}


function phoneNumber(noHP) {
    if (noHP.charAt(0) == '0') {
        return `+62${noHP.substring(1, 11)}`;
    } else {
        return noHP;
    }
}


function clearInputBox() {
    $("#firstNameInput").val("");
    $("#lastNameInput").val("");
    $("#phoneInput").val("");
    $("#dateInput").val("");
    $("#salaryInput").val("");
    $("#emailInput").val("");
    $("#genderInput").val("");
    $("#passwordInput").val("");
    $("#degreeInput").val("");
    $("#gpaInput").val("");
    $("#universityInput").val("");
}

function getGender(gender) {
    if (gender == 0) {
        return " Male";
    } else {
        return " Female";
    }
}

function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

/*$('#modalEdit').on('hidden.bs.modal', function () {
    location.reload();
})*/

/*let animals = [
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "simba", species: "cat", class: { name: "mamalia" } },
    { name: "tori", species: "cat", class: { name: "mamalia" } },
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "budi", species: "cat", class: { name: "mamalia" } }
]

for (let i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name == "Non-Mamalia";
    }
}
console.log(animals);

let OnlyCat = [];
for (let i = 0; i < animals.length; i++) {
    if ((animals[i].species = "cat")) {
        OnlyCat.push(animals[i]);
    }
}
console.log(OnlyCat);*/

/*    $.ajax({
        url: "https://swapi.dev/api/people/",
        success: function (result) {
            console.log(result.results);
            var text="";
            $.each(result.results, function (key, val){
                text += `<tr>
                            <th>${key+1}</th>
                            <td>${val.name}</td>
                            <td>${val.height}</td>
                            <td>${val.mass}</td>
                            <td>${val.hair_color}</td>
                            <td>${val.skin_color}</td>
                            <td>${val.eye_color}</td>
                            <td>${val.birth_year}</td>
                            <td>${val.gender}</td>
                         </tr>`;
            });
            console.log(text);
            $('.tableSW').html(text);
        }
    })*/

/*$(document).ready(function () {
  
    $('#datatableSW').DataTable({
        "ajax": {
            "url": "https://pokeapi.co/api/v2/pokemon",
            "dataType": "json",
            "dataSrc": "results"
        },
        "columns": [
            {
                "data": null, "sortable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": "name"
            },
            {
                "data": "",
                "data": null,
                render: function (data, type, row) {
                    return `<button class= "btn btn-primary" onclick="detailPoke('${row['url']}')" data-toggle="modal" data-target="#exampleModal">Detail</bottom>`;
                }
            }
        ]

    });
        
});*/


/*function detailPoke(url) {
    contentAbility = "";
    contentModal = "";
    getType = "";
    $.ajax({
        url: url,
        success: function (result) {
            result.abilities.forEach(abi => {
                let arr = abi.ability.name;
                contentAbility += `${arr}, `
            });

            result.types.forEach(ty => {
                if (ty.type.name == "grass") {
                    getType += `<h5 class="px-1"><span class="badge badge-pill badge-success">${ty.type.name}</span></h5>`
                }
                else if (ty.type.name == "poison")
                {
                    getType += `<h5 class="px-1"><span class="badge badge-pill badge-warning">${ty.type.name}</span></h5>`
                }
                else if (ty.type.name == "fire")
                {
                    getType += `<h5 class="px-1"><span class="badge badge-pill badge-danger">${ty.type.name}</span></h5>`
                }
                else if (ty.type.name == "normal")
                {
                    getType += `<h5 class="px-1"><span class="badge badge-pill badge-info">${ty.type.name}</span></h5>`
                }
                else 
                {
                    getType += `<h5 class="px-1"><span class="badge badge-pill badge-dark">${ty.type.name}</span></h5>`
                }
            });

            contentModal += `
              <div class="modal-header">
                <h5 class="modal-title text-capitalize" id="exampleModalLabel">${result.name}</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>

                  <div class="modal-body">
                    <div class="row">
                        <div class="col-md text-center">
                            <img class="img-thumbnail rounded-circle shadow p-3" width="190px" src="${result.sprites.other.dream_world.front_default}">
                         </div>
                    </div>
                     
                    <div class="row mt-3">
                         <div class="col-sm d-flex justify-content-center">
                              <p>${getType}</p>
                          </div>
                    </div>

                    <div class="row mt-4">
                        <div class="col-md">
                            <div class="card shadow-sm rounded px-1">
                                <table class="table table-sm">
                                  <thead>
                                     <tr>
                                       <th colspan="2" class="text-center">Ability</th>
                                     </tr>
                                  </thead>
                                  <tbody>
                                     <tr>
                                        <th>Ability</th>
                                        <td>: ${contentAbility}</td>
                                     </tr>
                                     <tr>
                                        <th>Weight</th>
                                        <td>: ${result.weight}</td>
                                     </tr>
                                     <tr>
                                        <th>Height</th>
                                        <td>: ${result.height}</td>
                                     </tr>
                                 </tbody>
                            </table>
                            </div>
                        </div>

                        <div class="col-md">
                            <div class="card shadow-sm rounded px-1">
                                <table class="table table-sm">
                              <thead>
                                <tr>
                                  <th colspan="2" class="text-center">Stats</th>
                                </tr>
                              </thead>
                              <tbody>
                                <tr>
                                    <th width="45%">HP</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-success progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[0].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[0].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                   <th width="45%">Attack</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-danger progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[1].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[1].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <th width="45%">Defence</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-warning progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[2].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[2].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th width="45%">Special Attack</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-info progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[3].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[3].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <th width="45%">Special Defence</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-primary progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[4].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[4].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                     <th width="45%">Speed</th>
                                    <td>
                                        <div class="progress">
                                          <div class="progress-bar bg-secondary progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${result.stats[5].base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${result.stats[5].base_stat}%</div>
                                        </div>
                                    </td>
                                </tr>
                              </tbody>
                            </table>
                           </div> 
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
              </div> `
            $('.modal-content').html(contentModal);
        }
    })
}*/

/*jquery form validate*/
/*$("#myForm").validate({
    rules: {
        
        email: {
            email: true
        },
        password: {
            minlength: 5
        },
        confirm_password: {
            equalTo:"#password"
        },
        phone: {
            number: true,
            minlength: 10,
            maxlength: 10
        }
    },
    messages: {
        firstName: {
            required: "Please enter your firstname",
            minlength: "at least 5 characters",
        },
        lastName: {
            required: "Please enter your lastname",
            minlength: "at least 5 characters",
        },
        email: {
            required: "Please enter your email",
            email:"Please enter a valid email address"
        },
        password: {
            required: "Please provide a password",
            minlength: "Your password must be at least 5 characters long"
        },
        confirm_password: {
            equalTo: "Please enter the same password as beside"
        },
        phone: "please enter your phone",
        formMessage: "Please enter your message"
    },
    submitHandler: function (form) {
        form.submit();
    },
});*/

/* date time picker*/

/*$('#datepicker').datepicker({
    uiLibrary: 'bootstrap4'
});*/





   