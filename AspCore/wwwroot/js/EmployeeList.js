(function () {
    'use strict';
    var EmployeeViewModel;

    function EmployeeViewModel() {
        var self;
        self = this;

        self.pageNumber = ko.observable(1);
        self.pageSize = ko.observable(10);
        self.pages = ko.observableArray([]);
        self.employeeList = ko.observableArray([]);

        self.getList = function (data, event) {
            var url = $.rootDir + 'Employees/ListEmployee/' + (self.pageNumber() - 1) + '/' + self.pageSize();
            $.ajax({
                type: "GET",
                url: url,
                //data: $('#searchListForm').serialize(),
                success: function (data) {
                    var items = ko.utils.arrayMap(data.data, function (item) {
                        var row = {};
                        for (var p in item) {
                            row[p.charAt(0).toLowerCase() + p.substr(1)] = ko.observable(item[p]);
                        }
                        return row;
                    });
                    self.employeeList(items);
                    self.pages(ko.utils.range(1, data.pageCount));
                },
                error: function (xhr) {
                }
            });
        };
        self.getList();

        //Paging    
        self.selectedPages = ko.computed(function () {
            var i, minPage, maxPage, sps;
            minPage = Math.floor((self.pageNumber() - 1) / 10) * 10 + 1;
            maxPage = Math.min(self.pages().length + 1, minPage + 10);
            sps = [];
            for (i = minPage; i < maxPage; i++) {
                sps.push(i);
            }
            return sps;
        });

        self.gotoPage = function (page) {
            if (self.pages.indexOf(page) !== -1 && page !== self.pageNumber()) {
                self.pageNumber(page);
                self.getList();
            }
        };
        self.prevPage = function () {
            self.gotoPage(self.pageNumber() - 1);
        };
        self.prevTenPage = function () {
            if (self.pageNumber() - 10 < self.pages()[0]) {
                self.gotoPage(self.pages()[0]);
            } else {
                self.gotoPage(self.pageNumber() - 10);
            }
        };
        self.nextPage = function () {
            self.gotoPage(self.pageNumber() + 1);
        };
        self.nextTenPage = function () {
            if (self.pageNumber() + 10 > self.pages()[self.pages().length - 1]) {
                self.gotoPage(self.pages()[self.pages().length - 1]);
            } else {
                self.gotoPage(self.pageNumber() + 10);
            }
        };     

        self.GetClientReport = function (data, event) {
            window.open('/Employees/GetReport/' + data.employeeId(), "_blank");
        }
    };

    EmployeeViewModel = new EmployeeViewModel();
    ko.applyBindings(EmployeeViewModel, document.getElementById('employee'));

}());

var koModel = ko.contextFor($('#employee')[0]).$data;
$(function () {
});