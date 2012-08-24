/// <reference path="https://api.trello.com/1/client.js?key=cba437c57ff37b1d42536e654657490d" />
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

ko.bindingHandlers.myFader = {
    init: function (element) {
        $(element).fadeTo(0, 0.5);
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        if (ko.utils.unwrapObservable(value)) { $(element).fadeTo(1000, 1); }
    }
};

function AuthViewModel() {
    var self = this;
    self.authed = ko.observable(false);
    self.exists = ko.observable(false);
    self.authToken = ko.observable("");
    self.githubUser = ko.observable("");
    self.bitbucketUser = ko.observable("");
    self.chosenBoard = ko.observable();
    self.boards = ko.observableArray([]);

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
            self.exists(true);
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
            expiration: "never"
        })
    };

    self.onAuthorize = function () {
        console.log("OnAuthorize");
        self.authToken(Trello.token());
        console.log("self.authToken= " + self.authToken());
        $.getJSON("api/authorization/" + self.authToken()).done(function (data) {
            console.log("Got " + data);
            self.githubUser(data.GithubUser);
            self.bitbucketUser(data.BitbucketUser);
            self.exists(true);
        }).always(function () {
            console.log("Loaded");
            self.authed(true);
            Trello.get("members/me/boards", function (boards) {
                self.boards(boards);
            });
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



