function InfoRowModel(data) {
    var self = this;

    self.IdLog = ko.observable(data.IdLog || undefined);
    self.Date = ko.observable(data.Date || undefined);
    self.Name = ko.observable(data.Name || undefined);
    self.DniType = ko.observable(data.DniType || undefined);
    self.DniNumber = ko.observable(data.DniNumber || undefined);
    self.Sex = ko.observable(data.Sex || undefined);
    self.Amount = ko.observable(data.Amount || undefined);
    self.Quotas = ko.observable(data.Quotas || undefined);
    self.Status = ko.observable(data.Status || undefined);
}

function CreditViewModel() {
    var self = this;

    self.IdentityValidatorVM = ko.observable(new IdentityValidatorViewModel({}));

    self.CreditFormVM = ko.validatedObservable(new CreditFormViewModel({}));

    self.enableTable = ko.observable(true);
    self.enableIdentity = ko.observable(false);
    self.enableDetailRow = ko.observable(false);
    self.credits = ko.observableArray();
    self.enablePrint = ko.observable();

    self.enableViewSendButton = ko.observable(false);
    self.enableViewExitButton = ko.observable(false);

    self.resultStage = ko.observable();

    self.Init = function () {

        Object.keys(self).forEach(function (name) {
            var b = self[name]

            if (ko.isWritableObservable(b)) {

                if (self[name] == self.CreditFormVM) {
                    self.CreditFormVM().Init();
                    self.CreditFormVM().enableViewSendButton(true);
                    return;
                }

                if (self[name] == self.IdentityValidatorVM) {
                    self.IdentityValidatorVM().Init();
                    return;
                }
                //self[name](undefined);
            }
        });

        self.enableTable(true);
        self.enableIdentity(false);
        self.enableDetailRow(false);

        self.enableViewSendButton(false);
        self.enableViewExitButton(false);

        self.LoadInfo();
    }

    self.LoadInfo = function () {

        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: "GetCredits",
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        self.credits(data.list);
                    }
                    else {
                        Utils.showErrorModal(data.error);
                    }
                }
            },
            error: function (xhr) {
                Utils.showErrorModal(xhr.responseText);
            }
        });

    }

    self.selectRow = function (item) {

        switch (item.Status) {
            case window.messages.CommercePreAuthorized:
                var result = self.IdentityValidatorVM().GetIdentification(item.DniNumber, item.Sex, false, item.IdLog);

                if (result != undefined) {
                    if (result == window.messages.A) {
                        self.CreditFormVM().EnableAllFieldStatus();
                        self.CreditFormVM().enableViewSendButton(true);
                        self.enablePrint(false);

                        self.setFormSelection(item.IdLog);
                    }
                    else {
                        self.enableTable(false);
                        self.enableIdentity(true);
                    }
                }
                break;
            case window.messages.OKPreAuthorized:
                self.CreditFormVM().DisableAllFieldStatus();
                self.CreditFormVM().enableViewSendButton(false);
                self.enablePrint(true);

                self.setFormSelection(item.IdLog);
                break;
            case window.messages.PendingPreAuthorized:
                self.CreditFormVM().DisableAllFieldStatus();
                self.CreditFormVM().enableViewSendButton(false);
                self.enablePrint(false);

                self.setFormSelection(item.IdLog);
                break;
        }
    };

    self.Print = function () {
        self.CreditFormVM().Print();
    }

    self.Back = function (item) {
        self.Init();
    };

    self.Refresh = function () {
        self.LoadInfo();
    }

    self.GetFormCommand = function () {
        self.enableTable(false);
        self.enableIdentity(false);
        self.enableDetailRow(true);

        self.CreditFormVM().setCurrentSelection(self.CreditFormVM().creditDetail().Id());

        if (self.CreditFormVM().creditDetail().Status() == window.messages.CommercePreAuthorized) {
            self.CreditFormVM().EnableAllFieldStatus();
            self.CreditFormVM().enableViewSendButton(true);
        }
        else {
            self.CreditFormVM().DisableAllFieldStatus();
            self.CreditFormVM().enableViewSendButton(false);
        }

    }

    //** helper

    self.setFormSelection = function (idlog) {
        self.CreditFormVM().setCurrentSelection(idlog);

        self.enableTable(false);
        self.enableDetailRow(true);
    }

    //** Subscribe 

    self.IdentityValidatorVM().result.subscribe(function (value) {
        if (value == window.messages.A) {
            self.resultStage(true);
        }
        if (value == window.messages.R) {
            self.resultStage(false);
        }
    });

    self.resultStage.subscribe(function (value) {
        if (value) {
            self.enableViewSendButton(true);
        }
        else {
            self.enableViewSendButton(false);
        }
    });
}

$(function () {

    ko.validation.rules.pattern.message = 'Invalid.';

    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        errorClass: 'errorStyle',
        messageTemplate: null
    }, true);

    ko.bindingHandlers.enterkey = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {

            var inputSelector = 'input,textarea,select';

            $(document).on('keypress', inputSelector, function (e) {
                var allBindings = allBindingsAccessor();
                $(element).on('keypress', 'input, textarea, select', function (e) {
                    var keyCode = e.which || e.keyCode;
                    if (keyCode !== 13) {
                        return true;
                    }

                    return false;
                });
            });
        }
    };

    var model = new CreditViewModel();
    var node = $("#content")[0];

    model.Init();

    Utils.initViewModel(node, model);

})
