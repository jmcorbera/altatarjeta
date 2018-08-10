function FormModel(data) {
    var self = this;

    self.MaxSpecifiedItemAmount1 = ko.observable(7);
    self.MaxSpecifiedItemAmount2 = ko.observable(7);
    self.MaxSpecifiedItemAmount3 = ko.observable(7);

    self.EnableTel1Input = ko.observable(false);
    self.EnableTel2Input = ko.observable(false);
    self.EnableTel3Input = ko.observable(false);

    self.Tel1IsSelected = ko.observable(false);
    self.Tel2IsSelected = ko.observable(false);
    self.Tel3IsSelected = ko.observable(false);

    self.Id = ko.observable(data.Id || undefined);
    self.Status = ko.observable(data.Status || undefined);
    self.Subsidiary = ko.observable(data.Subsidiary || undefined);
    self.IdSeller = ko.observable(data.IdSeller || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Seller = ko.observable(data.Seller || undefined);
    self.Amount = ko.observable(data.Amount || undefined);
    self.Plan = ko.observable(data.Plan || undefined);
    self.IdQuota = ko.observable(data.IdQuota || undefined);
    self.Quota = ko.observable(data.Quota || undefined);
    self.TaxQuota = ko.observable(data.TaxQuota || undefined);
    self.Name = ko.observable(data.Name || undefined);
    self.DNI = ko.observable(data.DNI || undefined);
    self.Sex = ko.observable(data.Sex || undefined);
    if (data.Sex) {
        self.Sex = ko.observable(data.Sex == "M" ? window.messages.Male : window.messages.Female);
    }
    else {
        self.Sex = ko.observable(undefined);
    }

    if (data.BirthDate) {
        self.BirthDate = ko.observable(new Date(parseInt(data.BirthDate.replace('/Date(', ''))).toLocaleDateString()).extend({
            required: { message: window.messages.MessageRequired },
            date: true,
            min: { message: window.messages.MessageMinLength, params: new Date(Date.now() - (31557600000 * 80)) },
            max: { message: window.messages.MessageMaxLength, params: new Date(Date.now() - (31557600000 * 21)) }
        });
        //(Date.now() - parseInt(data.list.BirthDate.replace('/Date(', ''))) / 31557600000
    }
    else {
        self.BirthDate = ko.observable(undefined).extend({
            required: { message: window.messages.MessageRequired },
            date: true,
            min: { message: window.messages.MaxAgeCredit, params: new Date(Date.now() - (31557600000 * 80)) },
            max: { message: window.messages.MinAgeCredit, params: new Date(Date.now() - (31557600000 * 21)) }
        });
    }
    //self.Age = { message: window.messages.MessageRequired }

    self.IdMaritalStatus = ko.observable(data.IdMaritalStatus || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.MaritalStatus = ko.observable(data.MaritalStatus || undefined);
    self.IdNationality = ko.observable(data.IdNationality || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Nationality = ko.observable(data.Nationality || undefined);
    self.Address = ko.observable(data.Address || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.AddressNumber = ko.observable(data.AddressNumber || undefined).extend({
        required: { message: window.messages.MessageRequired },
        minLength: { message: window.messages.MessageMinLength, params: 2 },
        maxLength: { message: window.messages.MessageMaxLength, params: 5 },
        number: { message: window.messages.MessageNumber, params: true }
    });
    self.PostalCode = ko.observable(data.PostalCode || undefined).extend({ required: { message: window.messages.MessageRequired } });
    //self.BetweenStreets = ko.observable(data.BetweenStreets || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.IdLocation = ko.observable(data.IdLocation || undefined);
    self.Location = ko.observable(data.Location || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.IdProvince = ko.observable(data.IdProvince || undefined);
    self.Province = ko.observable(data.Province || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.IdHousing = ko.observable(data.IdHousing || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Housing = ko.observable(data.Housing || undefined);
    self.Email = ko.observable(data.Email || undefined).extend({
        required: { message: window.messages.MessageRequired },
        email: { message: window.messages.MessageMail, params: true }
    });

    self.AreaCode1 = ko.observable(data.AreaCode1 || undefined);
    self.Tel1 = ko.observable(data.Tel1 || undefined).extend({
        required: {
            onlyIf: self.areaCode1,
            message: window.messages.MessageRequired
        },
        minLength: { message: window.messages.telNumberLong, params: self.MaxSpecifiedItemAmount1 },
        number: { message: window.messages.MessageNumber, params: true }
    });
    self.IdTelType1 = ko.observable(data.IdTelType1 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelType1 = ko.observable(data.TelType1 || undefined);
    self.IdTelRel1 = ko.observable(data.IdTelRel1 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelRel1 = ko.observable(data.TelRel1 || undefined);

    self.AreaCode2 = ko.observable(data.AreaCode2 || undefined);
    self.Tel2 = ko.observable(data.Tel2 || undefined).extend({
        required: {
            onlyIf: self.areaCode2,
            message: window.messages.MessageRequired
        },
        minLength: { message: window.messages.telNumberLong, params: self.MaxSpecifiedItemAmount2 },
        number: { message: window.messages.MessageNumber, params: true }
    });
    self.IdTelType2 = ko.observable(data.IdTelType2 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelType2 = ko.observable(data.TelType2 || undefined);
    self.IdTelRel2 = ko.observable(data.IdTelRel2 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelRel2 = ko.observable(data.TelRel2 || undefined);

    self.AreaCode3 = ko.observable(data.AreaCode3 || undefined);
    self.Tel3 = ko.observable(data.Tel3 || undefined).extend({
        required: {
            onlyIf: self.areaCode3,
            message: window.messages.MessageRequired
        },
        minLength: { message: window.messages.telNumberLong, params: self.MaxSpecifiedItemAmount3 },
        number: { message: window.messages.MessageNumber, params: true }
    });
    self.IdTelType3 = ko.observable(data.IdTelType3 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelType3 = ko.observable(data.TelType3 || undefined);
    self.IdTelRel3 = ko.observable(data.IdTelRel3 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelRel3 = ko.observable(data.TelRel3 || undefined);

    self.IdOcupation = ko.observable(data.IdOcupation || undefined);
    self.Ocupation = ko.observable(data.Ocupation || undefined);

    self.Obs = ko.observable(data.Obs || undefined);

    self.LaboralData = ko.observable(new LaboralFormModel({}));

}

function LaboralFormModel(data) {
    var self = this;

    self.WorkMaxSpecifiedItemAmount = ko.observable(7);

    self.EnableWorkTelInput = ko.observable(false);

    self.WorkTelIsSelected = ko.observable(false);

    self.Company = ko.observable(data.Company || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.CUIT = ko.observable(data.CUIT || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkFile = ko.observable(data.WorkFile || undefined);
    self.IdJobField = ko.observable(data.IdJobField || undefined);
    self.JobField = ko.observable(data.JobField || undefined);

    if (data.AdmissionDate) {
        self.AdmissionDate = ko.observable(new Date(parseInt(data.AdmissionDate.replace('/Date(', '')))).extend({
            required: { message: window.messages.MessageRequired },
            date: true,
            max: { message: window.messages.WorkSeniority, params: new Date(Date.now() - (31557600000 / 2)) },
        });
    }
    else {
        self.AdmissionDate = ko.observable(undefined).extend({
            required: { message: window.messages.MessageRequired },
            date: true,
            max: { message: window.messages.WorkSeniority, params: new Date(Date.now() - (31557600000 / 2)) },
        });
    }

    self.MontlyIncome = ko.observable(data.MontlyIncome || undefined);

    self.WorkAreaCode = ko.observable(data.AreaCode || undefined);
    self.WorkingTel = ko.observable(data.WorkingTel || undefined).extend({
        required: {
            onlyIf: self.WorkAreaCode,
            message: window.messages.MessageRequired
        },
        minLength: { message: window.messages.MessageMinLength, params: self.WorkMaxSpecifiedItemAmount },
        maxLength: { message: window.messages.MessageMaxLength, params: self.WorkMaxSpecifiedItemAmount },
        number: { message: window.messages.MessageNumber, params: true }
    });

    self.WorkAddress = ko.observable(data.WorkAddress || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkAddressNumber = ko.observable(data.WorkAddressNumber || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkPostalCode = ko.observable(data.WorkPostalCode || undefined).extend({ required: { message: window.messages.MessageRequired } });
    //self.WorkBetweenStreets = ko.observable(data.WorkBetweenStreets || undefined);
    self.IdWorkLocation = ko.observable(data.IdWorkLocation || undefined);
    self.WorkLocation = ko.observable(data.WorkLocation || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.IdWorkProvince = ko.observable(data.IdWorkProvince || undefined);
    self.WorkProvince = ko.observable(data.WorkProvince || undefined).extend({ required: { message: window.messages.MessageRequired } });
}

function CreditFormViewModel() {

    var self = this;

    self.FullAddress = ko.observable('');
    self.Lat = ko.observable('lat');
    self.Lon = ko.observable('lon');

    self.WorkFullAddress = ko.observable('');
    self.WorkLat = ko.observable('lat');
    self.WorkLon = ko.observable('lon');

    self.creditDetail = ko.validatedObservable(new FormModel({}));

    self.currentPlan = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.currentQuota = ko.observable().extend({ required: { message: window.messages.MessageRequired } });

    self.errors = ko.validation.group(self);
    self.formErrors = ko.validation.group(self.creditDetail);
    self.laboralErrors = ko.validation.group(self.creditDetail().LaboralData);

    self.enableViewSendButton = ko.observable();
    self.enableLaboralData = ko.observable();

    //self.enableViewMap = ko.observable();
    //self.enableViewWorkMap = ko.observable();

    self.countErrors = ko.observable();

    //#region ComboTypes

    self.sellersList = ko.observable();
    self.plansList = ko.observable();
    self.quotasList = ko.observable();
    self.quotasPlanList = ko.observable();
    self.maritalStatusList = ko.observable();
    self.nationalityList = ko.observable();
    self.locationList = ko.observable();
    self.provinceList = ko.observable();
    self.housingList = ko.observable();
    self.telTypeList = ko.observable(); // TIPO_TEL  
    self.telRelList = ko.observable();  // TEQuien
    self.ocupationList = ko.observable();
    self.jobFieldList = ko.observable();

    self.areaCodeList = ko.observable();

    //#endregion

    self.fieldStatus = {

        'seller': ko.observable(false),
        'plan': ko.observable(false),
        'quota': ko.observable(false),
        'maritalStatus': ko.observable(false),
        'nationality': ko.observable(false),
        'location': ko.observable(false),
        'province': ko.observable(false),
        'housing': ko.observable(false),

        'subsidiary': ko.observable(false),
        'Amount': ko.observable(false),
        'TaxQuota': ko.observable(false),
        'Name': ko.observable(false),
        'DNI': ko.observable(false),
        'Sex': ko.observable(false),
        'BirthDate': ko.observable(false),
        'Address': ko.observable(false),
        'AddressNumber': ko.observable(false),
        'PostalCode': ko.observable(false),
        'BetweenStreets': ko.observable(false),
        'Email': ko.observable(false),
        'AreaCode1': ko.observable(false),
        'Tel1': ko.observable(false),
        'telType1': ko.observable(false),
        'telRel1': ko.observable(false),
        'AreaCode2': ko.observable(false),
        'Tel2': ko.observable(false),
        'telType2': ko.observable(false),
        'telRel2': ko.observable(false),
        'AreaCode3': ko.observable(false),
        'Tel3': ko.observable(false),
        'telType3': ko.observable(false),
        'telRel3': ko.observable(false),

        'ocupation': ko.observable(false),
        'Company': ko.observable(false),
        'CUIT': ko.observable(false),
        'WorkFile': ko.observable(false),
        'JobField': ko.observable(false),
        'AdmissionDate': ko.observable(false),
        'MontlyIncome': ko.observable(false),
        'WorkAreaCode': ko.observable(false),
        'WorkingTel': ko.observable(false),
        'WorkAddress': ko.observable(false),
        'WorkAddressNumber': ko.observable(false),
        'WorkPostalCode': ko.observable(false),
        'WorkBetweenStreets': ko.observable(false),
        'WorkLocation': ko.observable(false),
        'WorkProvince': ko.observable(false),

        'Obs': ko.observable(false),
    }

    self.Init = function () {
        Object.keys(self).forEach(function (name) {
            var b = self[name]

            if (ko.isWritableObservable(b)) {
                if (self[name] == self.creditDetail) {
                    return;
                }

                self[name](undefined);
            }
        });

        self.enableViewSendButton(false);
        //self.enableViewMap(false);
        self.errors.showAllMessages(false);
        self.formErrors.showAllMessages(false);
        self.laboralErrors.showAllMessages(false);
        self.DisableAllFieldStatus();
        self.fillAllList();
    }

    //self.ViewMap = function () {
    //    document.getElementById("map_canvas")
    //    self.enableViewMap(!self.enableViewMap());
    //}

    //self.ViewWorkMap = function () {
    //    self.enableViewWorkMap(!self.enableViewWorkMap());
    //}

    self.CancelCommand = function () {

        var data = {
            Id: self.creditDetail().Id(),
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

    self.fillAllList = function () {
        self.fillCustomList("EstadoCivil", self.maritalStatusList);
        self.fillCustomList("Nacionalidad", self.nationalityList);
        self.fillCustomList("Localidad", self.locationList);
        self.fillCustomList("Provincia", self.provinceList);
        self.fillCustomList("TipoVivienda", self.housingList);
        self.fillCustomList("TIPO_TEL", self.telTypeList);
        self.fillCustomList("TEQuien", self.telRelList);
        self.fillCustomList("Ocupacion", self.ocupationList);
        self.fillCustomList("RubroLaboral", self.jobFieldList);

        self.fillSellersList();
        self.fillPlansList();
        self.fillQuotasList();

        self.fillAreaCode();
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

    self.fillSellersList = function () {
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: 'GetSellers',
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        self.sellersList(data.list);
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

    self.fillPlansList = function () {
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: 'Getplans',
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        self.plansList(data.list);
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

    self.fillQuotasList = function () {
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: 'GetQuotas',
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        self.quotasList(data.list);
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

    self.fillAreaCode = function () {
        //var url = UrlAction('Home', 'GetAreaCodes');
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: 'GetAreaCodes',
            success: function (data) {
                if (data.success) {
                    self.areaCodeList = data.list.map(function (element) {
                        // JQuery.UI.AutoComplete expects label & value properties, but we can add our own
                        return {
                            label: element.AreaCode,
                            value: element.Id,
                            // This way we still have acess to the original object
                            object: element
                        };
                    });
                }
                else {
                    Utils.showErrorModal(data.error);
                }
            },
            error: function (xhr) {
                Utils.showErrorModal(xhr.responseText);
            }
        });
    }

    self.setCurrentSelection = function (idLog) {
        var errors = 0;

        $.ajax({
            async: true,
            type: "POST",
            beforeSend: function () {
                $('#ajaxLoaderModal').modal('show');
            },
            url: "GetCreditDetail",
            data: { 'idLog': idLog },
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {

                        for (var prop in data.list) {
                            if (self.creditDetail()[prop] != undefined) {
                                if (data.list[prop] != null) {
                                    if (data.list[prop].length > 5) {
                                        if (data.list[prop].substring(1, 5) == "Date") {
                                            var date = new Date(parseInt(data.list[prop].replace('/Date(', '')));
                                            self.creditDetail()[prop](date);
                                        }
                                        else {
                                            self.creditDetail()[prop](data.list[prop]);
                                        }
                                    }
                                    else {
                                        self.creditDetail()[prop](data.list[prop]);
                                    }
                                }
                            }
                            if (ko.isWritableObservable(self.creditDetail().LaboralData()[prop])) {
                                if (data.list[prop] != null) {
                                    if (typeof (data.list[prop]) != "number") {
                                        if (data.list[prop].substring(1, 5) == "Date") {
                                            var date = new Date(parseInt(data.list[prop].replace('/Date(', '')));
                                            self.creditDetail().LaboralData()[prop](date);
                                        }
                                        else {
                                            self.creditDetail().LaboralData()[prop](data.list[prop]);
                                        }
                                    }
                                    else {
                                        self.creditDetail().LaboralData()[prop](data.list[prop]);
                                    }
                                }
                                else {
                                    self.creditDetail().LaboralData()[prop](undefined);
                                }
                            }
                        }

                        if (self.creditDetail().IdOcupation() == 12309) {
                            self.enableLaboralData(true);
                        }
                        else {
                            self.enableLaboralData(false);
                        }

                        self.currentPlan(self.creditDetail().Plan());
                        self.currentQuota(self.creditDetail().IdQuota());
                        self.creditDetail().Id(idLog);

                        //test

                        if (self.errors().length != 0 || self.formErrors().length != 0) {
                            errors = errors + self.errors().length + self.formErrors().length;
                        }

                        if (self.creditDetail().IdOcupation() == 12309) {
                            if (self.laboralErrors().length === 0) {
                                errors = errors + self.laboralErrors().length
                            }
                        }

                        self.countErrors(errors);

                    }
                    else {
                        Utils.showErrorModal(data.error);
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

        self.errors.showAllMessages(false);
        self.formErrors.showAllMessages(false);
        self.laboralErrors.showAllMessages(false);
    };

    self.EnableAllFieldStatus = function () {
        for (var prop in self.fieldStatus) {
            self.setFieldStatus(prop, true);
        }
    }

    self.DisableAllFieldStatus = function () {
        for (var prop in self.fieldStatus) {
            self.setFieldStatus(prop, false);
        }
    }

    self.setFieldStatus = function (key, enable) {
        self.fieldStatus[key](enable);
    }

    self.currentPlan.subscribe(function (value) {
        if (value) {
            self.quotasPlanList(self.quotasList());
        }
        else {
            self.quotasPlanList([]);
        }
    });

    self.currentQuota.subscribe(function (value) {
        if (value) {

            var obj = self.quotasPlanList();

            Object.keys(obj).forEach(function (key) {

                if (obj[key].Id == value) {

                    self.creditDetail().Quota(obj[key].Quota);

                    var quota = ((self.creditDetail().Amount() * (obj[key].Coefficient / 100)) + Number(self.creditDetail().Amount())) / obj[key].Quota;

                    self.creditDetail().TaxQuota(Number(quota).toFixed(2));

                }

            });
        }
        else {
            self.creditDetail().TaxQuota([]);
        }
    });

    self.creditDetail().AreaCode1.subscribe(function (newValue) {
        if (self.creditDetail().AreaCode1()) {
            self.creditDetail().MaxSpecifiedItemAmount1(self.creditDetail().AreaCode1().object.TelNumberLong);
            $("#TelNumber1")[0].maxLength = self.creditDetail().AreaCode1().object.TelNumberLong;
            self.creditDetail().EnableTel1Input(true);
            $("#TelNumber1")[0].focus();
        }
    });

    self.creditDetail().Tel1IsSelected.subscribe(function (newvalue) {
        if (!newvalue) {
            var obj = self.areaCodeList;
            var telNumberLong = "";

            Object.keys(obj).forEach(function (key) {
                if (obj[key].label == $("#Tel1AreaCode")[0].value) {
                    self.creditDetail().AreaCode1(obj[key]);
                    telNumberLong = obj[key].object.TelNumberLong;
                }
            });

            if (telNumberLong != "") {
                self.creditDetail().MaxSpecifiedItemAmount1(telNumberLong);
                $("#TelNumber1")[0].maxLength = telNumberLong;
                self.creditDetail().EnableTel1Input(true);
                $("#TelNumber1")[0].focus();
            }
            else {
                self.creditDetail().Tel1(undefined);
                self.creditDetail().EnableTel1Input(false);
            }
        }
        else {
            self.creditDetail().AreaCode1(undefined);
            self.creditDetail().Tel1(undefined);
            self.creditDetail().EnableTel1Input(false);
        }
    });

    self.creditDetail().AreaCode2.subscribe(function (newValue) {
        if (self.creditDetail().AreaCode2()) {
            self.creditDetail().MaxSpecifiedItemAmount2(self.creditDetail().AreaCode2().object.TelNumberLong);
            $("#TelNumber2")[0].maxLength = self.creditDetail().AreaCode2().object.TelNumberLong;
            self.creditDetail().EnableTel2Input(true);
            $("#TelNumber2")[0].focus();
        }
    });

    self.creditDetail().Tel2IsSelected.subscribe(function (newvalue) {
        if (!newvalue) {
            var obj = self.areaCodeList;
            var telNumberLong = "";

            Object.keys(obj).forEach(function (key) {
                if (obj[key].label == $("#Tel2AreaCode")[0].value) {
                    self.creditDetail().AreaCode2(obj[key]);
                    telNumberLong = obj[key].object.TelNumberLong;
                }
            });

            if (telNumberLong != "") {
                self.creditDetail().MaxSpecifiedItemAmount2(telNumberLong);
                $("#TelNumber2")[0].maxLength = telNumberLong;
                self.creditDetail().EnableTel2Input(true);
                $("#TelNumber2")[0].focus();
            }
            else {
                self.creditDetail().Tel2(undefined);
                self.creditDetail().EnableTel2Input(false);
            }
        }
        else {
            self.creditDetail().AreaCode2(undefined);
            self.creditDetail().Tel2(undefined);
            self.creditDetail().EnableTel2Input(false);
        }
    });

    self.creditDetail().AreaCode3.subscribe(function (newValue) {
        if (self.creditDetail().AreaCode3()) {
            self.creditDetail().MaxSpecifiedItemAmount3(self.creditDetail().AreaCode3().object.TelNumberLong);
            $("#TelNumber3")[0].maxLength = self.creditDetail().AreaCode3().object.TelNumberLong;
            self.creditDetail().EnableTel3Input(true);
            $("#TelNumber3")[0].focus();
        }
    });

    self.creditDetail().Tel3IsSelected.subscribe(function (newvalue) {
        if (!newvalue) {
            var obj = self.areaCodeList;
            var telNumberLong = "";

            Object.keys(obj).forEach(function (key) {
                if (obj[key].label == $("#Tel3AreaCode")[0].value) {
                    self.creditDetail().AreaCode3(obj[key]);
                    telNumberLong = obj[key].object.TelNumberLong;
                }
            });

            if (telNumberLong != "") {
                self.creditDetail().MaxSpecifiedItemAmount3(telNumberLong);
                $("#TelNumber3")[0].maxLength = telNumberLong;
                self.creditDetail().EnableTel3Input(true);
                $("#TelNumber3")[0].focus();
            }
            else {
                self.creditDetail().Tel3(undefined);
                self.creditDetail().EnableTel3Input(false);
            }
        }
        else {
            self.creditDetail().AreaCode3(undefined);
            self.creditDetail().Tel3(undefined);
            self.creditDetail().EnableTel3Input(false);
        }
    });

    self.creditDetail().LaboralData().WorkAreaCode.subscribe(function (newValue) {
        if (self.creditDetail().LaboralData().WorkAreaCode()) {
            self.creditDetail().LaboralData().WorkMaxSpecifiedItemAmount(self.creditDetail().LaboralData().WorkAreaCode().object.TelNumberLong);
            $("#WorkTelNumber")[0].maxLength = self.creditDetail().LaboralData().WorkAreaCode().object.TelNumberLong;
            self.creditDetail().LaboralData().EnableWorkTelInput(true);
            $("#WorkTelNumber")[0].focus();
        }
    });

    self.creditDetail().LaboralData().WorkTelIsSelected.subscribe(function (newvalue) {
        if (!newvalue) {
            var obj = self.areaCodeList;
            var telNumberLong = "";

            Object.keys(obj).forEach(function (key) {
                if (obj[key].label == $("#WorkAreaCode")[0].value) {
                    self.creditDetail().LaboralData().WorkAreaCode(obj[key]);
                    telNumberLong = obj[key].object.TelNumberLong;
                }
            });

            if (telNumberLong != "") {
                self.creditDetail().LaboralData().WorkMaxSpecifiedItemAmount(telNumberLong);
                $("#WorkAreaCode")[0].maxLength = telNumberLong;
                self.creditDetail().LaboralData().EnableWorkTelInput(true);
                $("#WorkAreaCode")[0].focus();
            }
            else {
                self.creditDetail().LaboralData().WorkingTel(undefined);
                self.creditDetail().LaboralData().EnableWorkTelInput(false);
            }
        }
        else {
            self.creditDetail().LaboralData().WorkAreaCode(undefined);
            self.creditDetail().LaboralData().WorkingTel(undefined);
            self.creditDetail().LaboralData().EnableWorkTelInput(false);
        }
    });

    self.Submit = function () {
        var errors = 0;

        if (self.errors().length != 0 || self.formErrors().length != 0) {
            errors = errors + self.errors().length + self.formErrors().length;
        }

        if (self.creditDetail().IdOcupation() == 12309) {
            if (self.laboralErrors().length != 0) {
                errors = errors + self.laboralErrors().length
            }
        }

        self.countErrors(errors);

        if (errors === 0) {
            var status = self.creditDetail().Status();

            if (self.creditDetail().Status() == window.messages.CommercePreAuthorized)
                status = window.messages.OKPreAuthorized;

            var data = {
                Id: self.creditDetail().Id(),
                Plan: self.currentPlan(),
                Status: status,
                IdSeller: self.creditDetail().IdSeller(),
                Amount: self.creditDetail().Amount(),
                IdQuota: self.currentQuota(),
                Quota: self.creditDetail().Quota(),
                TaxQuota: self.creditDetail().TaxQuota(),
                IdMaritalStatus: self.creditDetail().IdMaritalStatus(),
                IdNationality: self.creditDetail().IdNationality(),
                BirthDate: self.creditDetail().BirthDate(),
                Address: self.creditDetail().Address(),
                AddressNumber: self.creditDetail().AddressNumber(),
                PostalCode: self.creditDetail().PostalCode(),
                //BetweenStreets: self.creditDetail().BetweenStreets(),
                Location: self.creditDetail().Location(),
                Province: self.creditDetail().Province(),
                IdHousing: self.creditDetail().IdHousing(),
                Email: self.creditDetail().Email(),
                AreaCode1: self.creditDetail().AreaCode1(),
                Tel1: self.creditDetail().Tel1(),
                IdTelType1: self.creditDetail().IdTelType1(),
                IdTelRel1: self.creditDetail().IdTelRel1(),
                AreaCode2: self.creditDetail().AreaCode2(),
                Tel2: self.creditDetail().Tel2(),
                IdTelType2: self.creditDetail().IdTelType2(),
                IdTelRel2: self.creditDetail().IdTelRel2(),
                AreaCode3: self.creditDetail().AreaCode3(),
                Tel3: self.creditDetail().Tel3(),
                IdTelType3: self.creditDetail().IdTelType3(),
                IdTelRel3: self.creditDetail().IdTelRel3(),
                IdOcupation: self.creditDetail().IdOcupation(),

                Company: self.creditDetail().LaboralData().Company(),
                CUIT: self.creditDetail().LaboralData().CUIT(),
                WorkFile: self.creditDetail().LaboralData().WorkFile(),
                IdJobField: self.creditDetail().LaboralData().IdJobField(),
                AdmissionDate: self.creditDetail().LaboralData().AdmissionDate(),
                MontlyIncome: self.creditDetail().LaboralData().MontlyIncome(),
                WorkAreaCode: self.creditDetail().LaboralData().WorkAreaCode(),
                WorkingTel: self.creditDetail().LaboralData().WorkingTel(),
                WorkAddress: self.creditDetail().LaboralData().WorkAddress(),
                WorkAddressNumber: self.creditDetail().LaboralData().WorkAddressNumber(),
                WorkPostalCode: self.creditDetail().LaboralData().WorkPostalCode(),
                //WorkBetweenStreets: self.creditDetail().LaboralData().WorkBetweenStreets(),
                WorkLocation: self.creditDetail().LaboralData().WorkLocation(),
                WorkProvince: self.creditDetail().LaboralData().WorkProvince(),

                Obs: self.creditDetail().Obs(),
            }

            $.ajax({
                type: "POST",
                async: false,
                cache: false,
                url: "SaveForm",
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
                            self.DisableAllFieldStatus();
                            self.enableViewSendButton(false);

                            if (self.creditDetail().Status() == "PCO") {
                                var modal = Utils.showInfoModal(window.messages.FormSaved);
                                modal.find('.accept').on('click', function (e) {
                                    var url = UrlAction('Home', 'PrintForm');
                                    var w = window.open(url, "popupwindow", "width=800,height=700,left=200,top=5,scrollbars,toolbar=0,resizable").document.title = "Formulario PDF";
                                });
                            }
                            else {
                                Utils.showInfoModal(window.messages.FormSavedPending);
                            }
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
        else {
            Utils.showInfoModal(window.messages.CheckData);
            self.errors.showAllMessages();
            self.formErrors.showAllMessages();
            self.laboralErrors.showAllMessages();
        }
    }
}


$(function () {

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

    ko.validation.makeBindingHandlerValidatable('datepicker');

    ko.bindingHandlers.autoComplete = {
        // Only using init event because the Jquery.UI.AutoComplete widget will take care of the update callbacks
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // { selected: mySelectedOptionObservable, options: myArrayOfLabelValuePairs }
            var settings = valueAccessor();

            var selectedOption = settings.selected;
            var options = settings.options;

            var updateElementValueWithLabel = function (event, ui) {
                // Stop the default behavior
                event.preventDefault();

                // Update our SelectedOption observable
                if (typeof ui.item !== "undefined") {
                    // ui.item - label|value|...
                    if (ui.item != null) {
                        // Update the value of the html element with the label 
                        // of the activated option in the list (ui.item)
                        $(element).val(ui.item.label);
                    }
                    selectedOption(ui.item);
                }
            };

            $(element).autocomplete({
                source: options,
                select: function (event, ui) {
                    updateElementValueWithLabel(event, ui);
                }
            });
        }
    };

    ko.bindingHandlers.addressAutocomplete = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            var options = {
                types: ['geocode'],
                componentRestrictions: { country: 'ar' }
            };

            //var center = new google.maps.LatLng(-34.541158, -58.715695)

            ko.utils.extend(options, allBindings.autocompleteOptions)

            //var map_options = {
            //    center: center,
            //    zoom: 17,
            //    mapTypeId: google.maps.MapTypeId.ROADMAP,
            //    fullscreenControl: false,
            //    mapTypeControl: false,
            //    streetViewControl: false
            //};

            try {

                //var map = new google.maps.Map(document.getElementById("map_canvas"), map_options);

                var autocomplete = new google.maps.places.Autocomplete(element, options);

                //var marker = new google.maps.Marker({ map: map });

                google.maps.event.addListener(autocomplete, 'place_changed', function () {
                    // Get the place details from the autocomplete object.
                    var result = autocomplete.getPlace();

                    for (var b in allBindings) {
                        if (allBindings.hasOwnProperty(b)) {
                            allBindings[b](undefined);
                        }
                    }

                    //if (result.geometry.viewport) {
                    //    map.fitBounds(result.geometry.viewport);
                    //} else {
                    //    map.setCenter(result.geometry.location);
                    //}

                    //map.setZoom(17);

                    //marker.setPosition(result.geometry.location);

                    value(result.formatted_address);

                    if (result.geometry) {
                        allBindings['lat'](result.geometry.location.lat());
                        allBindings['lon'](result.geometry.location.lng());
                    }

                    // The following section poplutes any bindings that match an address component with a first type that is the same name
                    // administrative_area_level_1, posatl_code etc. these can be found in the Google Places API documentation
                    var components = _(result.address_components).groupBy(function (c) { return c.types[0]; });
                    _.each(_.keys(components), function (key) {
                        if (allBindings.hasOwnProperty(key)) {
                            if (key == 'postal_code') {
                                var postal = components[key][0].short_name.substr(1, 4);
                                allBindings[key](postal);
                            }
                            else {
                                allBindings[key](components[key][0].short_name);
                            }
                        }
                    });

                });
            }
            catch (err) {
                Utils.showErrorModal(err);
            }

        },
        update: function (element, valueAccessor, allBindingsAccessor) {
            ko.bindingHandlers.value.update(element, valueAccessor);
        }
    };

    ko.bindingHandlers.workaddressAutocomplete = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            var options = {
                types: ['geocode'],
                componentRestrictions: { country: 'ar' }
            };

            //var center = new google.maps.LatLng(-34.541158, -58.715695)

            ko.utils.extend(options, allBindings.autocompleteOptions)

            //var map_options = {
            //    center: center,
            //    zoom: 17,
            //    mapTypeId: google.maps.MapTypeId.ROADMAP,
            //    fullscreenControl: false,
            //    mapTypeControl: false,
            //    streetViewControl: false
            //};

            try {

                //var map = new google.maps.Map(document.getElementById("workmap_canvas"), map_options);

                var autocomplete = new google.maps.places.Autocomplete(element, options);

                //var marker = new google.maps.Marker({ map: map });

                google.maps.event.addListener(autocomplete, 'place_changed', function () {
                    // Get the place details from the autocomplete object.
                    var result = autocomplete.getPlace();

                    for (var b in allBindings) {
                        if (allBindings.hasOwnProperty(b)) {
                            allBindings[b](undefined);
                        }
                    }

                    //if (result.geometry.viewport) {
                    //    map.fitBounds(result.geometry.viewport);
                    //} else {
                    //    map.setCenter(result.geometry.location);
                    //}

                    //map.setZoom(17);

                    //marker.setPosition(result.geometry.location);

                    value(result.formatted_address);

                    if (result.geometry) {
                        allBindings['lat'](result.geometry.location.lat());
                        allBindings['lon'](result.geometry.location.lng());
                    }

                    // The following section poplutes any bindings that match an address component with a first type that is the same name
                    // administrative_area_level_1, posatl_code etc. these can be found in the Google Places API documentation
                    var components = _(result.address_components).groupBy(function (c) { return c.types[0]; });
                    _.each(_.keys(components), function (key) {
                        if (allBindings.hasOwnProperty(key)) {
                            if (key == 'postal_code') {
                                var postal = components[key][0].short_name.substr(1, 4);
                                allBindings[key](postal);
                            }
                            else {
                                allBindings[key](components[key][0].short_name);
                            }
                        }
                    });

                });
            }
            catch (err) {
                Utils.showErrorModal(err);
            }

        },
        update: function (element, valueAccessor, allBindingsAccessor) {
            ko.bindingHandlers.value.update(element, valueAccessor);
        }
    };

})