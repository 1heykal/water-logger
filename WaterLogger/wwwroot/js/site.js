function calculate(){
    var tbl = document.getElementById("records");
    let result = 0;
    var resultArea = document.getElementById("result");

    for (var i = 1; i < tbl.rows.length; i++) {
        result += Number(tbl.rows[i].cells[1].innerHTML);
    }

    resultArea.innerHTML = `<h4>Total is ${result}</h4>`;
}