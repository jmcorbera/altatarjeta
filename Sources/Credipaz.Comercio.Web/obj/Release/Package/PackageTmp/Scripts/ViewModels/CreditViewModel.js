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

    self.FullAddress = ko.observable('t');
    self.Street = ko.observable('s');
    self.Suburb = ko.observable('c');
    self.State = ko.observable('r');
    self.Lat = ko.observable("Lat");
    self.Lon = ko.observable("Lon");


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

    ko.bindingHandlers.addressAutocomplete = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            var options = { types: ['geocode'] };
            //ko.utils.extend(options, allBindings.autocompleteOptions)

            var autocomplete = new google.maps.places.Autocomplete(element);

            autocomplete.addListener(autocomplete, 'place_changed', fillInAddress());

            function fillInAddress() {
                // Get the place details from the autocomplete object.
                var place = autocomplete.getPlace();

                if (place) {
                    alert(place.formatted_address);

                    // Get each component of the address from the place details
                    // and fill the corresponding field on the form.
                    for (var i = 0; i < place.address_components.length; i++) {
                        var addressType = place.address_components[i].types[0];
                        if (componentForm[addressType]) {
                            var val = place.address_components[i][componentForm[addressType]];
                            document.getElementById(addressType).value = val;
                        }
                    }
                }


            };

        },
        update: function (element, valueAccessor, allBindingsAccessor) {
            ko.bindingHandlers.value.update(element, valueAccessor);
        }
    };



    var model = new CreditViewModel();
    var node = $("#content")[0];

    model.Init();

    Utils.initViewModel(node, model);

})
