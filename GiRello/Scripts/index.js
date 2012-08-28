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
    self.trelloUserId = ko.observable();
    self.authToken = ko.observable();
    self.githubUser = ko.observable();
    self.bitbucketUser = ko.observable();
    self.chosenBoard = ko.observable();
    self.boards = ko.observableArray([]);

    self.saveAuth = function () {
        self.blockUI();
        var method = "post";
        var u = "";
        if (self.exists()) {
            method = "put";
            u = "/" + self.trelloUserId();
        }
        $.ajax({
            url: "api/Authorization" + u,
            type: method,
            contentType: "application/json",
            data: ko.toJSON({ TrelloUserId: self.trelloUserId, Token: self.authToken, GithubUser: self.githubUser, BitbucketUser: self.bitbucketUser })
        }).done(function () {
            console.log("celebrate");
            self.exists(true);
        }).fail(function () {
            console.log("mope");
        }).always(function () {
            self.unblockUI();
        });
    };

    self.Authorize = function () {
        self.blockUI();
        Trello.authorize({
            type: 'popup',
            success: self.onAuthorize,
            name: "GiRello",
            scope: { write: true, read: true },
            expiration: "never"
        })
    };

    self.onAuthorize = function () {
        self.authToken(Trello.token());
        Trello.get("members/me?fields=username&boards=open&board_fields=name", function (me) {
            self.boards(me.boards);
            self.trelloUserId(me.id);
            $.getJSON("api/authorization/" + self.trelloUserId()).done(function (data) {
                self.githubUser(data.GithubUser);
                self.bitbucketUser(data.BitbucketUser);
                self.exists(true);
            }).always(function () {
                self.authed(true);
                self.unblockUI();
            });
        }, function () {
            Trello.deauthorize();
            self.unblockUI();
        });

    };

    self.blockUI = function () {
        $.blockUI({
            message: "<h1>Please wait...</h1>",
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#ffffff'
            }
        });
    };

    self.unblockUI = function () {
        $.unblockUI();
    };

    self.blockUI();
    Trello.authorize({
        interactive: false,
        success: this.onAuthorize,
        error: this.unblockUI
    });
}

$(document).ready(function () {
    console.log("Applying bindings");
    ko.applyBindings(new AuthViewModel());
});



