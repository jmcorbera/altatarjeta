function viewModel() {
    var self = this;

    self.IdentityValidatorVM = ko.observable(new IdentityValidatorViewModel({}));

    self.CreditFormVM = ko.observable(new CreditFormViewModel({}));

    self.enableRegister = ko.observable();
    self.enableIdentity = ko.observable();
    self.enableForm = ko.observable();

    self.docNumber = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.monthlyIncome = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.requestedAmount = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.sexList = ko.observable();
    self.sex = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.ocupationList = ko.observable();
    self.ocupation = ko.observable().extend({ required: { message: window.messages.MessageRequired } });

    self.resultStage = ko.observable();

    self.enableViewSendButton = ko.observable();
    self.enableViewExitButton = ko.observable();

    self.enableReturn = ko.observable();

    self.Init = function () {

        Object.keys(self).forEach(function (name) {
            var b = self[name]

            if (ko.isWritableObservable(b)) {
                if (self[name] == self.CreditFormVM) {
                    self.CreditFormVM().Init();
                    return;
                }

                if (self[name] == self.IdentityValidatorVM) {
                    self.IdentityValidatorVM().Init();
                    return;
                }
                self[name](undefined);
            }
        });

        self.errors = ko.validation.group(self);

        self.enableRegister(true);
        self.enableIdentity(false);
        self.enableForm(false);
        self.enableViewSendButton(false);
        self.enableViewExitButton(false);
        self.enableReturn(false);

        self.fillAllList();
        self.errors.showAllMessages(false);
    }

    self.Back = function (item) {
        self.Init();
    };

    self.fillAllList = function () {
        self.fillCustomList("sexo", self.sexList);
        self.fillCustomList("ocupacion", self.ocupationList);
    }

    self.fillCustomList = function (type, list) {
        var sex = type;
        var list = list;
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            data: { 'type': type },
            url: 'GetType',
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        list(data.list);
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

    self.CancelCommand = function () {
        if (!self.enableRegister()) {
            var data = {
                Id: self.CreditFormVM().creditDetail().Id(),
                Status: window.messages.Annulled,
            }

            $.ajax({
                type: "POST",
                async: false,
                cache: false,
                url: 'SaveForm',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.redirectTo) {
                        var modal = Utils.showInfoModal(window.messages.SessionExpired);
                        modal.find('.accept').on('click', function (e) {
                            window.location.href = data.redirectTo;
                        });
                    }
                    else {
                        if (data.success) {
                            self.Init();
                            Utils.showInfoModal(window.messages.PreAuthorizedCreditAnulled);
                        }
                        else {
                            self.Init();
                            Utils.showErrorModal(data.error);
                        }
                    }
                },
                error: function (xhr) {
                    self.Init();
                    Utils.showErrorModal(xhr.responseText);
                }
            });
        }
        else {
            self.Init();
        }
    }

    self.SubmitRegister = function () {
        if (this.errors().length === 0) {
            this.errors.showAllMessages();

            var CreditModel = {
                DniNumber: self.docNumber(),
                Sex: self.getItemById(self.sexList(), self.sex()).Code,
                Ocupation: self.getItemById(self.ocupationList(), self.ocupation()).Description,
                MonthlyIncome: self.monthlyIncome(),
                RequestedAmount: self.requestedAmount(),
            }

            $.ajax({
                async: true,
                type: "POST",
                beforeSend: function () {
                    $('#ajaxLoaderModal').modal('show');
                },
                url: 'GetAuthorization',
                data: JSON.stringify({ creditModel: CreditModel }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.redirectTo) {
                        var modal = Utils.showInfoModal(window.messages.SessionExpired);
                        modal.find('.accept').on('click', function (e) {
                            window.location.href = data.redirectTo;
                        });
                    }
                    else {
                        if (data.success) {
                            self.CreditFormVM().creditDetail().Id(data.idLog);

                            Utils.showInfoModal(window.messages.PreAuthorizedCredit)
                            var result = self.IdentityValidatorVM().GetIdentification(self.docNumber(), self.getItemById(self.sexList(), self.sex()).Code.substring(0, 1), true, data.idLog);

                            if (result == window.messages.A) {
                                self.GetFormCommand();
                            }
                            else {
                                self.enableRegister(false);
                                self.enableIdentity(true);
                            }
                        }
                        else {

                            Utils.showErrorModal(data.error);
                            self.Init();
                        }
                    }
                },
                error: function (xhr) {
                    Utils.showErrorModal(xhr.responseText);
                },
                complete: function () {
                    $('#ajaxLoaderModal').modal('hide');
                }
            });

        }
        else {
            Utils.showInfoModal(window.messages.CheckData);
            this.errors.showAllMessages();
        }
    }

    self.GetFormCommand = function () {
        self.enableRegister(false);
        self.enableIdentity(false);
        self.enableForm(true);

        self.CreditFormVM().setCurrentSelection(self.CreditFormVM().creditDetail().Id());

        self.CreditFormVM().EnableAllFieldStatus();
        self.CreditFormVM().enableViewSendButton(true);
    }

    self.getItemById = function (list, Id) {
        return ko.utils.arrayFirst(list, function (item) {
            return item.Id == Id;
        }) || "";
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
            self.enableReturn(false);
        }
        else {
            self.enableViewSendButton(false);
            self.enableReturn(true);
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
        messageTemplate: null
    }, true);

    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || { language: 'es', clearBtn: 'true' };
            $(element).datepicker(options);

            //when a user changes the date, update the view model
            ko.utils.registerEventHandler(element, "changeDate", function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    value(event.date);
                }
            });
        },
        update: function (element, valueAccessor) {
            var widget = $(element).data("datepicker");

            var value = ko.utils.unwrapObservable(valueAccessor());

            //when the view model is updated, update the widget
            if (widget) {
                widget.date = value;
                //if (widget.date) {
                    widget.setValue();
                    $(element).datepicker('update', value)
                //}
            }
        }
    };

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

    var model = new viewModel();
    var node = $("#content")[0];

    model.Init();

    Utils.initViewModel(node, model);

})