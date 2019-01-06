$(function () {
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status == 403) {
            alert("Your session is timeout. Please login again. thanks!");
            //window.location.href = "index.php"
            temp = document.location.pathname.split("/");
            newurl = document.location.origin + "/" + temp[1] + "/Navbar/LandingPage";
            window.location.href = newurl;
            //newurl = document.location.origin + "/" + temp[1] + "/Account/SessionTimeout";
            //window.location.href = newurl;


            //var response = $.parseJSON(xhr.responseText);
            ////window.location = response.RedirectUrl;
            //var $form = $(document.createElement('form')).css({ display: 'none' }).attr("method", "POST").attr("action", response.RedirectUrl);
            //$("body").append($form);
            //$form.submit();
        }
    });
});