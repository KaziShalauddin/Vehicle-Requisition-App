$(document).ready(function () {
    $("#DivisionDD").change(function () {
        
        debugger;

        var selectedDivisionId = $("#divisionDD").val();
        var jsonData = { divisionId: selectedDivisionId };

        

        $.ajax({
            url: "/district/GetByDivision",
            data: jsonData,
            type: "POST",
            success:function(response) {
                $("#districtDD").empty();

                var options = "<option>Select.....</option>";
                $.each(response,
                    function(key, district) {
                        options += "<option value='" + district.Id + "'>" + district.Name + "</option>";
                    });
                $("#districtDD").append(options);
            },
            error: function (response) {
                
            }

        });
    });
});