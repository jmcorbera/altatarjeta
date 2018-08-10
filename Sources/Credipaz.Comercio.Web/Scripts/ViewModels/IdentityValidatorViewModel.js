function QuestionModel(data) {
    var self = this;

    self.Id = ko.observable(data.Id || undefined);
    self.Name = ko.observable(data.Name || undefined);
    self.Answers = ko.observable(data.Answers || undefined);
}

function QueryModel(data) {
    var self = this;

    self.Id = ko.observable(data.Id || undefined);
    self.RejectedValidation = ko.observable(data.RejectedValidation || undefined);
    self.Locked = ko.observable(data.Locked || undefined);
    self.ValidationTimeOut = ko.observable(data.ValidationTimeOut || undefined);

    self.Questions = ko.observableArray(data.Questions || undefined);
}

function SaveQuestionsModel(data) {
    var self = this;

    self.End = ko.observable(data.End || undefined);
    self.Questions = ko.observable(data.Questions || undefined);
    self.CorrectQuestions = ko.observable(data.CorrectQuestions || undefined);
    self.HitsPercentage = ko.observable(data.HitsPercentage || undefined);
    self.HitsPercentageAcquired = ko.observable(data.HitsPercentageAcquired || undefined);

    if (data.Result) {
        self.Result = ko.observable(data.Result == window.messages.A ? window.messages.Correct : window.messages.Incorrect);
    }
    else {
        self.Result = ko.observable(undefined);
    }

    self.ReferenceCode = ko.observable(data.ReferenceCode || undefined);
    self.ResponseTime = ko.observable(data.ResponseTime || undefined);
}

function IdentityValidatorViewModel() {
    var self = this;

    self.idLog = ko.observable();

    self.result = ko.observable();
    self.showResult = ko.observable();

    self.exitData = ko.observable();

    self.dataCuil = ko.observable();
    self.dataName = ko.observable();
    self.itemChecked = ko.observable(); 

    self.timer = ko.observable();
    self.timerCountDown = ko.observable();

    self.query = ko.observable();
    self.currentQuestion = ko.observable();
    self.saveQuestionsModel = ko.observable();
    self.currentQIndex = ko.observable();

    self.enableViewQuestions = ko.observable();
    self.enableViewResultQuestions = ko.observable();

    self.enableViewConfirmButton = ko.observable();
    self.enableViewQuestionButton = ko.observable();
    self.enableViewCountDown = ko.observable();

    self.confirmDataStage = ko.observable();
    self.validationStage = ko.observable();

    self.Init = function () {

        Object.keys(self).forEach(function (name) {
            var b = self[name]

            if (ko.isWritableObservable(b)) {
                if (self[name] == self.query) {
                    self.query(new QueryModel({}));
                    return;
                }

                if (self[name] == self.currentQuestion) {
                    self.currentQuestion(new QuestionModel({}));
                    return;
                }

                if (self[name] == self.saveQuestionsModel) {
                    self.saveQuestionsModel(new SaveQuestionsModel({}));
                    return;
                }

                self[name](undefined);
            }

        });

        self.timerCountDown(0);
        self.currentQIndex(0);

        self.enableViewQuestions(true);
        self.enableViewResultQuestions(false);
        self.enableViewCountDown(false);
    }

    self.CancelCommand = function (value) {

        var status = window.messages.Annulled

        if (value == window.messages.RejectPreAuthorized)
            status = value

        var data = {
            Id: self.idLog(),
            Status: status
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
                        self.result(window.messages.R);
                        self.enableViewConfirmButton(false);
                        self.enableViewQuestionButton(false);
                        self.enableViewCountDown(false);
                        if (status == window.messages.Annulled)
                            Utils.showInfoModal(window.messages.PreAuthorizedCreditAnulled);

                    }
                    else {
                        //self.Init();
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

    self.GetIdentification = function (doc, sex, showresult, idLog) {

        self.Init();

        self.idLog(idLog);
        self.showResult(showresult);

        var data = {
            Doc: doc,
            Sex: sex, 
        }

        $.ajax({
            type: "POST",
            async: false,
            cache: false,
            url: 'GetIdentification',
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
                        if (data.saveQuestionsModel == null) {
                            self.dataCuil(data.cuil);
                            self.dataName(data.name);

                            self.confirmDataStage(true);
                        }
                        else {
                            self.result(data.saveQuestionsModel.Result);
                            self.dataCuil(data.cuil);
                            self.dataName(data.name);
                            self.saveQuestionsModel(new SaveQuestionsModel(data.saveQuestionsModel));
                            self.ShowResult();
                            if(self.showResult())
                                Utils.showInfoModal(window.messages.ValidationExist);
                        }
                    }
                    else {
                        self.result(window.messages.R);
                        self.CancelCommand(window.messages.RejectPreAuthorized);
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

        return self.result();
    }

    self.SubmitIdentity = function () {
        if (self.confirmDataStage()) {
            self.GetQuestions();
        }
        else {
            if (self.currentQIndex() > 0) {
                self.ProcessQuestion(false);
            }
            else {
                self.GetQuestions();
            }
        }
    }

    self.GetQuestions = function () {
        $.ajax({
            type: "POST",
            async: false,
            cache: false,
            url: 'GetQuestions',
            data: { 'cuil': self.dataCuil() },
            success: function (data) {
                if (data.redirectTo) {
                    var modal = Utils.showInfoModal(window.messages.SessionExpired);
                    modal.find('.accept').on('click', function (e) {
                        window.location.href = data.redirectTo;
                    });
                }
                else {
                    if (data.success) {
                        if (data.queryModel.RejectedValidation == "N") {
                            self.query(new QueryModel(data.queryModel));
                            self.ProcessQuestion(true);

                            self.confirmDataStage(false);
                            self.validationStage(true);
                        }
                        else {
                            self.result(window.messages.R);
                            self.enableViewConfirmButton(false);
                            self.enableViewQuestionButton(false);
                            self.CancelCommand(window.messages.RejectPreAuthorized);
                            Utils.showErrorModal(window.messages.ValidationRejected);
                            //self.Init();
                        }
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

    self.ProcessQuestion = function (firstQuestion) {
        if (firstQuestion == true) {
            var id = self.query().Questions()[self.currentQIndex()].Id;
            self.currentQuestion(new QuestionModel(self.GetQuestionById(id)));

            self.timerCountDown(self.query().ValidationTimeOut());
            self.InitTimer();

            self.enableViewCountDown(true);

            self.currentQIndex(self.currentQIndex() + 1);
        }
        else {
            if (self.itemChecked() != null) {
                if (self.exitData() == null) {
                    self.exitData(self.currentQuestion().Id() + ";" + self.itemChecked());
                }
                else {
                    self.exitData(self.exitData() + "|" + self.currentQuestion().Id() + ";" + self.itemChecked());
                }

                if (self.query().Questions().length != (self.currentQIndex())) {
                    var id = self.query().Questions()[self.currentQIndex()].Id;
                    self.currentQuestion(new QuestionModel(self.GetQuestionById(id)));
                    self.currentQIndex(self.currentQIndex() + 1);
                }
                else {
                    self.SaveQuestions();
                }

                self.itemChecked(null);
            }
            else {
                Utils.showErrorModal(window.messages.NoOptionSelected);
            }
        }
    }

    self.SaveQuestions = function () {

        clearInterval(self.timer());

        var data = {
            QueryId: self.query().Id(),
            Questions: self.exitData(),
            Cuil: self.dataCuil(),
        }

        $.ajax({
            type: "POST",
            async: false,
            cache: false,
            url: 'SaveQuestions',
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {
                    self.saveQuestionsModel(new SaveQuestionsModel(data.saveQuestionsModel));

                    if (self.saveQuestionsModel().End() == "N") {
                        self.GetQuestions();
                    }
                    else {
                        self.result(data.saveQuestionsModel.Result);
                        if (window.messages.R)
                        {
                            self.CancelCommand(window.messages.RejectPreAuthorized);
                        }
                        
                        self.ShowResult();
                    }
                }
                else {
                    self.Init();
                    Utils.showErrorModal(data.error);

                }
            },
            error: function (xhr) {
                self.Init();
                Utils.showErrorModal(xhr.responseText);
            }
        });

    };

    self.ShowResult = function () {
        self.enableViewResultQuestions(true);
        self.validationStage(false);
        self.enableViewQuestions(false);

    }

    self.ShowQuestions = function () {
        self.enableViewResultQuestions(false);
        self.enableViewQuestions(true);
    }

    //**Helpers

    self.GetQuestionById = function (questionId) {
        return Utils.getItemById(self.query().Questions(), questionId);
    }

    self.InitTimer = function () {
        var sec = self.timerCountDown();

        self.timer(setInterval(function () {
            self.timerCountDown(--sec);
            if (sec == 0) {
                clearInterval(self.timer());
                Utils.showInfoModal(window.messages.TimeExpired);
                self.Init();
            }

        }, 1000));
    }

    //** Subscribe 

    self.confirmDataStage.subscribe(function (value) {
        if (value) {
            self.enableViewConfirmButton(true);
        }
        else {
            self.enableViewConfirmButton(false);
        }
    });

    self.validationStage.subscribe(function (value) {
        if (value) {
            self.enableViewQuestionButton(true);
        }
        else {
            self.enableViewQuestionButton(false);
        }
    });

}