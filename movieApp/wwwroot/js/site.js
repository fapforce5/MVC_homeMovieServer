var site = {};

$(document).ready(function () {
    site.getLoggedInUser();
});

site.switchUser = function (userId, pageRedirect = null) {
    $("#site_login").html("Switching...");
    $.ajax({
        url: "/User/SwitchUser",
        type: "POST",
        data: { userId: userId },
        dataType: "json",
        success: function (d) {
            if (pageRedirect === null)
                window.location.reload();
            else
                window.location.replace(pageRedirect);

        },
        error: function (ex) {
            console.log(ex);
        }
    });
};


site.GetAllUsers = function () {
    $("#site_login_wrapper").slideDown();
    $("#site_login").html("Loading...");
    $.ajax({
        url: "/User/GetAllUsers",
        type: "GET",
        dataType: "json",
        success: function (d) {
            $("#site_login").html("");
            $.each(d, function (i, v) {
                //site_login
                $("#site_login").append('<button type="button" class="user-icon text-white" onclick="site.switchUser(' + v.userId + ')">' +
                    '<img src="/images/userIcons/' + v.filename + '"/>' +
                    v.username + 
                    '</button>');
            });

            $("#site_login").append('<button type="button" class="user-icon text-primary" onclick="location.href=\'/user\';">' +
                '<img src="data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=">' +
                'Add User' +
                '</button>');
            $("#site_login").append('<button type="button" class="user-icon text-primary" onclick="site.switchUser(null)">' +
                '<img src="data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=">' +
                'Log out' + 
                '</button>');
            $("#site_login").append('<button type="button" class="user-icon site-userSelect text-danger" onclick="site.closeGetAllUsers()">' +
                '<img src="data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=">' +
                'Cancel' +
                '</button>');
        },
        error: function (ex) {
            console.log(ex);
        }
    });
};

site.closeGetAllUsers = function () {
    $("#site_login_wrapper").slideUp();
};

site.getLoggedInUser = function () {
    $.ajax({
        url: "/User/GetLoggedInUser",
        type: "GET",
        dataType: "json",
        success: function (d) {
            var user = d.user;
            var userIcon = d.userIcon;
            $("#site_user_name").text(user.username);
            $("#site_user_icon").attr("src", "/images/userIcons/" + userIcon.filename);
        },
        error: function (ex) {
            console.log(ex);
        }
    });
};

site.setSelectLogin = function () {
    site_login
}

site.padZero = function (num) {
    return num < 10 ? "0" + num.toString() : num.toString();
};