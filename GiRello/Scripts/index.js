ko.bindingHandlers.myText = {
    init: function (element, valueAccessor) {
        // Start visible/invisible according to initial value
        var shouldDisplay = valueAccessor();
        shouldDisplay ? $(element).removeAttr('disabled') : $(element).attr('disabled', true);
    },
    update: function (element, valueAccessor) {
        // On update, fade in/out
        var shouldDisplay = valueAccessor();
        shouldDisplay ? $(element).removeAttr('disabled') : $(element).attr('disabled', true);
    }
};

ko.bindingHandlers.jqButton = {
    init: function (element) {
        $(element).button(); // Turns the element into a jQuery UI button
    },
    update: function (element, valueAccessor) {
        var currentValue = valueAccessor();
        // Here we just update the "disabled" state, but you could update other properties too
        $(element).button("option", "disabled", currentValue.enable === false);
    }
};

function AuthViewModel() {
    var self = this;
    self.loaded = ko.observable(false);
    self.exists = ko.observable(false);
    self.authToken = ko.observable("");
    self.githubUser = ko.observable("");
    self.bitbucketUser = ko.observable("");

    self.saveAuth = function () {
        var d = ko.toJSON({ Token: self.authToken, GithubUser: self.githubUser, BitbucketUser: self.bitbucketUser });
        console.log(d);
        var method = "post";
        var u = "";
        if (self.exists()) {
            method = "put";
            u = "/" + self.authToken();
        }
        $.ajax({
            url: "api/Authorization" + u,
            type: method,
            contentType: "application/json",
            data: ko.toJSON({ Token: self.authToken, GithubUser: self.githubUser, BitbucketUser: self.bitbucketUser })
        }).done(function () {
            console.log("celebrate");
        }).fail(function () {
            console.log("mope");
        });
    };

    self.Authorize = function () {
        Trello.authorize({
            type: 'popup',
            success: self.onAuthorize,
            name: "GiRello",
            scope: { write: true, read: true },
            expiration: "never",
        })
    };

    self.onAuthorize = function () {
        console.log("OnAuthorize");
        self.authToken(Trello.token());
        console.log("this.AuthToken= " + self.authToken());
        $.getJSON("api/authorization/" + self.authToken()).done(function (data) {
            console.log("Got " + data);
            self.githubUser(data.GithubUser);
            self.bitbucketUser(data.BitbucketUser);
            self.exists(true);
        }).always(function () {
            console.log("Loaded");
            self.loaded(true);
        });
    };

    Trello.authorize({
        interactive: false,
        success: this.onAuthorize
    });
}

$(document).ready(function () {
    console.log("Applying bindings");
    ko.applyBindings(new AuthViewModel());
});



