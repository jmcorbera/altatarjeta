function FormModel(data) {
    var self = this;

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
        self.BirthDate = ko.observable(new Date(parseInt(data.BirthDate.replace('/Date(', ''))).toLocaleDateString());
    }
    else {
        self.BirthDate = ko.observable(undefined);
    }

    self.IdMaritalStatus = ko.observable(data.IdMaritalStatus || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.MaritalStatus = ko.observable(data.MaritalStatus || undefined);
    self.IdNationality = ko.observable(data.IdNationality || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Nationality = ko.observable(data.Nationality || undefined);
    self.Address = ko.observable(data.Address || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.AddressNumber = ko.observable(data.AddressNumber || undefined).extend({ required:  { message: window.messages.MessageRequired },
                                                                                 minLength: { message: window.messages.MessageMinLength, params: 2 },
                                                                                 maxLength: { message: window.messages.MessageMaxLength, params: 5 },
                                                                                 number:    { message: window.messages.MessageNumber, params: true }
    });
    self.PostalCode = ko.observable(data.PostalCode || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.BetweenStreets = ko.observable(data.BetweenStreets || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.IdLocation = ko.observable(data.IdLocation || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Location = ko.observable(data.Location || undefined);
    self.IdProvince = ko.observable(data.IdProvince || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Province = ko.observable(data.Province || undefined);
    self.IdHousing = ko.observable(data.IdHousing || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.Housing = ko.observable(data.Housing || undefined);
    self.Email = ko.observable(data.Email || undefined).extend({required: { message: window.messages.MessageRequired },
                                                                email: { message: window.messages.MessageMail, params: true }
    });
    self.Tel1 = ko.observable(data.Tel1 || undefined).extend({required: { message: window.messages.MessageRequired },
                                                              number:   { message: window.messages.MessageNumber, params: true }
    });
    self.IdTelType1 = ko.observable(data.IdTelType1 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelType1 = ko.observable(data.TelType1 || undefined);
    self.IdTelRel1 = ko.observable(data.IdTelRel1 || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.TelRel1 = ko.observable(data.TelRel1 || undefined);
    self.Tel2 = ko.observable(data.Tel2 || undefined);
    self.IdTelType2 = ko.observable(data.IdTelType2 || undefined);
    self.TelType2 = ko.observable(data.TelType2 || undefined);
    self.IdTelRel2 = ko.observable(data.IdTelRel2 || undefined);
    self.TelRel2 = ko.observable(data.TelRel2 || undefined);
    self.Tel3 = ko.observable(data.Tel3 || undefined);
    self.IdTelType3 = ko.observable(data.IdTelType3 || undefined);
    self.TelType3 = ko.observable(data.TelType3 || undefined);
    self.IdTelRel3 = ko.observable(data.IdTelRel3 || undefined);
    self.TelRel3 = ko.observable(data.TelRel3 || undefined);
    self.IdOcupation = ko.observable(data.IdOcupation || undefined);
    self.Ocupation = ko.observable(data.Ocupation || undefined);

    self.Obs = ko.observable(data.Obs || undefined);

    self.LaboralData = ko.observable(new LaboralFormModel({}));

}

function LaboralFormModel(data) {
    var self = this;

    self.Company = ko.observable(data.Company || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.CUIT = ko.observable(data.CUIT || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkFile = ko.observable(data.WorkFile || undefined);
    self.IdJobField = ko.observable(data.IdJobField || undefined);
    self.JobField = ko.observable(data.JobField || undefined);

    if (data.AdmissionDate) {
        self.AdmissionDate = ko.observable(new Date(parseInt(data.AdmissionDate.replace('/Date(', '')))).extend({ required: { message: window.messages.MessageRequired } });
    }
    else {
        self.AdmissionDate = ko.observable(undefined).extend({ required: { message: window.messages.MessageRequired } });
    }

    self.MontlyIncome = ko.observable(data.MontlyIncome || undefined);
    self.WorkingTel = ko.observable(data.WorkingTel || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkAddress = ko.observable(data.WorkAddress || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkAddressNumber = ko.observable(data.WorkAddressNumber || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkPostalCode = ko.observable(data.WorkPostalCode || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkBetweenStreets = ko.observable(data.WorkBetweenStreets || undefined);
    self.IdWorkLocation = ko.observable(data.IdWorkLocation || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkLocation = ko.observable(data.WorkLocation || undefined);
    self.IdWorkProvince = ko.observable(data.IdWorkProvince || undefined).extend({ required: { message: window.messages.MessageRequired } });
    self.WorkProvince = ko.observable(data.WorkProvince || undefined);
}

function CreditFormViewModel() {

    var self = this;

    self.creditDetail = ko.validatedObservable(new FormModel({}));

    self.currentPlan = ko.observable().extend({ required: { message: window.messages.MessageRequired } });
    self.currentQuota = ko.observable().extend({ required: { message: window.messages.MessageRequired } });

    self.errors = ko.validation.group(self);
    self.formErrors = ko.validation.group(self.creditDetail);
    self.laboralErrors = ko.validation.group(self.creditDetail().LaboralData);

    self.enableViewSendButton = ko.observable();
    self.enableLaboralData = ko.observable();

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
        'Tel1': ko.observable(false),
        'telType1': ko.observable(false),
        'telRel1': ko.observable(false),
        'Tel2': ko.observable(false),
        'telType2': ko.observable(false),
        'telRel2': ko.observable(false),
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

                //self[name](undefined);
            }
        });

        self.enableViewSendButton(false);
        self.errors.showAllMessages(false);
        self.formErrors.showAllMessages(false);
        self.laboralErrors.showAllMessages(false);
        self.DisableAllFieldStatus();
        self.fillAllList();
    }

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

    self.setCurrentSelection = function (idLog) {
        var errors = 0;

        $.ajax({
            type: "POST",
            async: false,
            cache: false,
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
                                    self.creditDetail()[prop](data.list[prop]);
                                }
                            }
                            if (ko.isWritableObservable(self.creditDetail().LaboralData()[prop])) {
                                if (data.list[prop] != null) {
                                    if (typeof (data.list[prop]) != "number")
                                    {
                                        if (data.list[prop].substring(1, 5) == "Date") {
                                            var date = new Date(parseInt(data.list[prop].replace('/Date(', '')));
                                            self.creditDetail().LaboralData()[prop](date);
                                        }
                                        else
                                        {
                                            self.creditDetail().LaboralData()[prop](data.list[prop]);
                                        }
                                    }
                                    else
                                    {
                                        self.creditDetail().LaboralData()[prop](data.list[prop]);
                                    }                              
                                }
                                else
                                {
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
                Address: self.creditDetail().Address(),
                AddressNumber: self.creditDetail().AddressNumber(),
                PostalCode: self.creditDetail().PostalCode(),
                BetweenStreets: self.creditDetail().BetweenStreets(),
                IdLocation: self.creditDetail().IdLocation(),
                IdProvince: self.creditDetail().IdProvince(),
                IdHousing: self.creditDetail().IdHousing(),
                Email: self.creditDetail().Email(),
                Tel1: self.creditDetail().Tel1(),
                IdTelType1: self.creditDetail().IdTelType1(),
                IdTelRel1: self.creditDetail().IdTelRel1(),
                Tel2: self.creditDetail().Tel2(),
                IdTelType2: self.creditDetail().IdTelType2(),
                IdTelRel2: self.creditDetail().IdTelRel2(),
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
                WorkingTel: self.creditDetail().LaboralData().WorkingTel(),
                WorkAddress: self.creditDetail().LaboralData().WorkAddress(),
                WorkAddressNumber: self.creditDetail().LaboralData().WorkAddressNumber(),
                WorkPostalCode: self.creditDetail().LaboralData().WorkPostalCode(),
                WorkBetweenStreets: self.creditDetail().LaboralData().WorkBetweenStreets(),
                IdWorkLocation: self.creditDetail().LaboralData().IdWorkLocation(),
                IdWorkProvince: self.creditDetail().LaboralData().IdWorkProvince(),

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

