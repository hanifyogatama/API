// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

    $.ajax({
        url: "https://swapi.dev/api/people/",
        success: function (result) {
            console.log(result.results);
            var text="";
            var no = 0;
            $.each(result.results, function (key, val){
                text += `<tr>
                            <th scope="row">${no+1}</th>
                            <td>${val.name}</td>
                            <td>${val.height}</td>
                            <td>${val.mass}</td>
                            <td>${val.hair_color}</td>
                            <td>${val.skin_color}</td>
                            <td>${val.eye_color}</td>
                            <td>${val.birth_year}</td>
                            <td>${val.gender}</td>
                         </tr>`;
                no++;
            });
            console.log(text);
            $('.tableSW').html(text);
        }
    })





