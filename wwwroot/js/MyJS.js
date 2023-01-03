var subject=document.getElementById("Subject").value;
var stdID=document.getElementById("studentID").value;
var stdFname=document.getElementById("fullName").value;
document.getElementById("ThongBao").innerHTML= "Chuyển ca thi " + subject + " cho sinh viên: " + stdFname + "-" + stdID;

if(subject = "THVPNC")
{
    $("#MySelect").append(new Option("Ca 1 (Thời gian: 8:00 AM - 8:50AM)", "Ca1"));
    $("#MySelect").append(new Option("Ca 2 (Thời gian: 9:00 AM - 9:50AM)", "Ca2"));
    $("#MySelect").append(new Option("Ca 3 (Thời gian: 10:00 AM - 10:50AM)", "Ca3"));
    $("#MySelect").append(new Option("Ca 4 (Thời gian: 11:00 AM - 11:50AM)", "Ca4"));
    $("#MySelect").append(new Option("Ca 5 (Thời gian: 13:30 PM - 14:20PM)", "Ca5"));
    $("#MySelect").append(new Option("Ca 6 (Thời gian: 14:30 PM - 15:20PM)", "Ca6"));
    $("#MySelect").append(new Option("Ca 7 (Thời gian: 15:30 PM - 16:20PM)", "Ca7"));
    $("#MySelect").append(new Option("Ca 8 (Thời gian: 16:30 PM - 17:20PM)", "Ca8"));
}
else if(subject = "TMDT")
{
    $("#MySelect").append(new Option("Ca 1 (Thời gian: 8:00 AM - 8:50AM)", "Ca1"));
    $("#MySelect").append(new Option("Ca 2 (Thời gian: 9:00 AM - 9:50AM)", "Ca2"));
    $("#MySelect").append(new Option("Ca 3 (Thời gian: 10:00 AM - 10:50AM)", "Ca3"));
    $("#MySelect").append(new Option("Ca 4 (Thời gian: 11:00 AM - 11:50AM)", "Ca4"));
}
else if(subject = "QTDA")
{
    $("#MySelect").append(new Option("Ca 1 (Thời gian: 13:30 PM - 14:20PM)", "Ca1"));
    $("#MySelect").append(new Option("Ca 2 (Thời gian: 14:30 PM - 15:20PM)", "Ca2"));
    $("#MySelect").append(new Option("Ca 3 (Thời gian: 15:30 PM - 16:20PM)", "Ca3"));
}
var caThi=document.getElementById("CaThi").value;
$("#MySelect").val(caThi).change();

function genderChanged(obj)
{
    var caThiUpdate = obj.value;
    $("#CaThi").val(caThiUpdate);
}
