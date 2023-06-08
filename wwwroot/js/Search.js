
//alert(subject);
// var stdID=document.getElementById("studentID").value;
// var stdFname=document.getElementById("fullName").value;


var caThi=document.getElementById("CaThi").value;
$("#MySelect").val(caThi).change();

function GetCathiBySubject(obj)
{
    var subject = $('#Subject option:selected').val();
    if(subject == "TMDT")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l4 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 7:00 AM - 7:50AM)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 8:00 AM - 8:50AM)</option>'
        content += '<option value="Ca3">Ca 3 (Thời gian: 9:00 AM - 9:50AM)</option>'
        content += '<option value="Ca4">Ca 4 (Thời gian: 10:00 AM - 10:50AM)</option>'
        content += '<option value="Ca5">Ca 5 (Thời gian: 13:00 PM - 13:50PM)</option>'
        content += '<option value="Ca6">Ca 6 (Thời gian: 14:30 PM - 14:50PM)</option>'
        content += '<option value="Ca7">Ca 7 (Thời gian: 15:30 PM - 15:50PM)</option>'
        content += '<option value="Ca8">Ca 8 (Thời gian: 16:30 PM - 16:50PM)</option>'
        content += '</select>';
        $('#CaThi').html(content)
        var content2 = '';
        content2 += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l2 w3-large w3-margin-right" id="MySelect" name="SubjectGroup" onchange="genderChanged(this)">'
        content2 += '<option value="10">Nhóm 10</option>'
        content2 += '<option value="11">Nhóm 11</option>'
        content2 += '<option value="12">Nhóm 12</option>'
        content2 += '<option value="13">Nhóm 13</option>'
        content2 += '<option value="14">Nhóm 14</option>'
        content2 += '<option value="15">Nhóm 15</option>'
        content2 += '<option value="16">Nhóm 16</option>'
        content2 += '<option value="17">Nhóm 17</option>'
        content2 += '<option value="18">Nhóm 18</option>'
        content2 += '<option value="19">Nhóm 19</option>'
        content2 += '<option value="20">Nhóm 20</option>'
        content2 += '<option value="21">Nhóm 21</option>'
        content2 += '<option value="22">Nhóm 22</option>'
        content2 += '</select>';
        $('#SubjectGroup').html(content2)
    }
    else if(subject == "THVPNC")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l4 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 8:30 AM - 9:20AM)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 9:30 AM - 10:20AM)</option>'
        content += '</select>';
        $('#CaThi').html(content)
        var content2 = '';
        content2 += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l2 w3-large w3-margin-right" id="MySelect" name="SubjectGroup" onchange="genderChanged(this)">'
        content2 += '<option value="100">Nhóm 100</option>'
        content2 += '<option value="101">Nhóm 101</option>'
        content2 += '</select>';
        $('#SubjectGroup').html(content2)
    }
    // else if(subject == "QTDA")
    // {
    //     var content = '';
    //     content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l4 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
    //     content += '<option value="Ca1">Ca 1 (Thời gian: 13:30 PM - 14:20PM)</option>'
    //     content += '<option value="Ca2">Ca 2 (Thời gian: 14:30 PM - 15:20PM)</option>'
    //     content += '<option value="Ca3">Ca 3 (Thời gian: 15:30 PM - 16:20PM)</option>'
    //     content += '</select>';
    //     $('#CaThi').html(content)
    //     var content2 = '';
    //     content2 += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l2 w3-large w3-margin-right" id="MySelect" name="SubjectGroup" onchange="genderChanged(this)">'
    //     content2 += '<option value="150">Nhóm 150</option>'
    //     content2 += '<option value="151">Nhóm 151</option>'
    //     content2 += '<option value="152">Nhóm 152</option>'
    //     content2 += '</select>';
    //     $('#SubjectGroup').html(content2)
    // }
    else if(subject == "JavaOOP")
    {
        var content = '';
        content += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l4 w3-large w3-margin-right" id="MySelect" name="CaThi" onchange="genderChanged(this)">'
        content += '<option value="Ca1">Ca 1 (Thời gian: 08:00 AM - 08:50 AM ngày 15/06)</option>'
        content += '<option value="Ca2">Ca 2 (Thời gian: 09:00 AM - 09:50 AM ngày 15/06)</option>'
        content += '<option value="Ca3">Ca 3 (Thời gian: 10:00 AM - 10:50 AM ngày 15/06)</option>'
        content += '<option value="Ca4">Ca 4 (Thời gian: 13:00 PM - 13:50 PM ngày 15/06)</option>'
        content += '<option value="Ca5">Ca 5 (Thời gian: 14:00 PM - 14:50 PM ngày 15/06)</option>'
        content += '<option value="Ca6">Ca 6 (Thời gian: 15:00 PM - 15:50 PM ngày 15/06)</option>'
        content += '<option value="Ca7">Ca 7 (Thời gian: 16:00 PM - 16:50 PM ngày 15/06)</option>'
        content += '<option value="Ca8">Ca 8 (Thời gian: 08:00 AM - 08:50 AM ngày 16/06)</option>'
        content += '<option value="Ca9">Ca 9 (Thời gian: 09:00 AM - 09:50 AM ngày 16/06)</option>'
        content += '<option value="Ca10">Ca 10 (Thời gian: 10:00 AM - 10:50 AM ngày 16/06)</option>'
        content += '<option value="Ca11">Ca 11 (Thời gian: 13:00 PM - 13:50 PM ngày 16/06)</option>'
        content += '<option value="Ca12">Ca 12 (Thời gian: 14:00 PM - 14:50 PM ngày 16/06)</option>'
        content += '<option value="Ca13">Ca 13 (Thời gian: 15:00 PM - 15:50 PM ngày 16/06)</option>'
        content += '</select>';
        $('#CaThi').html(content)
        var content2 = '';
        content2 += '<select class="w3-select w3-border w3-round w3-white w3-padding w3-mobile w3-col l2 w3-large w3-margin-right" id="MySelect" name="SubjectGroup" onchange="genderChanged(this)">'
        content2 += '<option value="01">Nhóm 01</option>'
        content2 += '<option value="02">Nhóm 02</option>'
        content2 += '<option value="03">Nhóm 03</option>'
        content2 += '<option value="04">Nhóm 04</option>'
        content2 += '<option value="05">Nhóm 05</option>'
        content2 += '<option value="06">Nhóm 06</option>'
        content2 += '<option value="07">Nhóm 07</option>'
        content2 += '<option value="08">Nhóm 08</option>'
        content2 += '<option value="09">Nhóm 09</option>'
        content2 += '<option value="10">Nhóm 10</option>'
        content2 += '<option value="11">Nhóm 11</option>'
        content2 += '<option value="12">Nhóm 12</option>'
        content2 += '<option value="13">Nhóm 13</option>'
        content2 += '<option value="14">Nhóm 14</option>'
        content2 += '<option value="15">Nhóm 15</option>'
        content2 += '<option value="16">Nhóm 16</option>'
        content2 += '<option value="17">Nhóm 17</option>'
        content2 += '<option value="18">Nhóm 18</option>'
        content2 += '<option value="19">Nhóm 19</option>'
        content2 += '<option value="20">Nhóm 20</option>'
        content2 += '<option value="21">Nhóm 21</option>'
        content2 += '<option value="22">Nhóm 22</option>'
        content2 += '<option value="23">Nhóm 23</option>'
        content2 += '<option value="100">Nhóm 100</option>'
        content2 += '</select>';
        $('#SubjectGroup').html(content2)
    }
}

function genderChanged(obj)
{
    var caThiUpdate = obj.value;
}