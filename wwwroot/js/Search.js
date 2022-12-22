
//alert(subject);
// var stdID=document.getElementById("studentID").value;
// var stdFname=document.getElementById("fullName").value;


var caThi=document.getElementById("CaThi").value;
$("#MySelect").val(caThi).change();

function GetCathiBySubject(obj)
{
    var subject = $('#Subject option:selected').val();
    if(subject == "THVPNC")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l5 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 8:00 AM - 8:50AM)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 9:00 AM - 9:50AM)</option>'
        content += '<option value="Ca3">Ca 3 (Thời gian: 10:00 AM - 10:50AM)</option>'
        content += '<option value="Ca4">Ca 4 (Thời gian: 11:00 AM - 11:50AM)</option>'
        content += '<option value="Ca5">Ca 5 (Thời gian: 13:30 PM - 14:20PM)</option>'
        content += '<option value="Ca6">Ca 6 (Thời gian: 14:30 PM - 15:20PM)</option>'
        content += '<option value="Ca7">Ca 7 (Thời gian: 15:30 PM - 16:20PM)</option>'
        content += '<option value="Ca8">Ca 8 (Thời gian: 16:30 PM - 17:20PM)</option>'
        content += '</select>';
        $('#CaThi').html(content)
    }
    else if(subject == "QTDA")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l5 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 8:00 AM - 8:50AM)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 9:00 AM - 9:50AM)</option>'
        content += '<option value="Ca3">Ca 3 (Thời gian: 10:00 AM - 10:50AM)</option>'
        content += '<option value="Ca4">Ca 4 (Thời gian: 11:00 AM - 11:50AM)</option>'
        content += '</select>';
        $('#CaThi').html(content)
    }
    else if(subject == "TMDT")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l5 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 13:30 PM - 14:20PM)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 14:30 PM - 15:20PM)</option>'
        content += '<option value="Ca3">Ca 3 (Thời gian: 15:30 PM - 16:20PM)</option>'
        content += '</select>';
        $('#CaThi').html(content)
    }
}

function genderChanged(obj)
{
    var caThiUpdate = obj.value;
}