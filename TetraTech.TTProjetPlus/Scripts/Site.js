function grid_dataSourceSync(e) {
    setTimeout(function () { e.sender.dataSource.sync(); });
} 
function grid_dataSourceRead(e) {
    setTimeout(function () { e.sender.dataSource.read(); });
} 

function grid_requestEnd(e) { 
    if (e.type !== 'read' && !e.response.Errors) {
        e.sender.read();
    }
}  
function grid_error(e, gridNameId, message) {
    if (e.errors) { 
        $.each(e.errors, function (key, value) {
            if ('errors' in value) { $.each(value.errors, function () { message += this + '\n'; }); }
        });
        alert(message);
    }
}
function gridcombobox_onKeydown(e, comboboxNameId) {
    if (e.keyCode === 9) {
        e.preventDefault(); 
    }
}

function saveGridState(e, grid, localStorageName) {
    e.preventDefault();
    var options = grid.getOptions();
    localStorage[localStorageName] = kendo.stringify(options);  
}

function loadGridState(e, grid, localStorageName) {
    e.preventDefault(); 
    var options = localStorage[localStorageName];
    var toolBar = $("#" + grid.element[0].id + " .k-grid-toolbar").html();
    if (toolBar) { toolBar = toolBar.replace("#", "\\#").replace("</scr", "<\\/scr"); }

    if (options) {
        var opt = JSON.parse(options);
        grid.dataSource.filter(null); grid.dataSource.sort(null);
        if (toolBar) {
             opt.toolbar = [{ template: toolBar }];
        }
        grid.setOptions(opt);   
    }
}
function saveGridAdvOptions(e, grid, localStorageName) {
    e.preventDefault(); 
    var hideColumnsList = [];
    $.each(grid.columns, function (ix, value) { if (value.hidden) { hideColumnsList.push(value.field); } });

    var filter = grid.dataSource.filter(); var sort = grid.dataSource.sort();
    var opt = {
        filter: filter ? filter : {},
        sort: sort ? sort : {},
        hideColumns: hideColumnsList
    };
    localStorage[localStorageName] = kendo.stringify(opt); 
}
function loadGridAdvOptions(e, grid, localStorageName) {
    e.preventDefault(); 
    var localOptions = localStorage[localStorageName];
    if (localOptions) {
        var opt = JSON.parse(localOptions);
        grid.dataSource.filter(opt.filter);
        grid.dataSource.sort(opt.sort);
         
        $.each(grid.columns, function (ix, value) { grid.showColumn(value.field); });
        setTimeout(function () {
            saveColumnState = true;
            $.each(opt.hideColumns, function (ix, value) { grid.hideColumn(value); });
        }, 1000);
    }
} 


function kendoFastReDrawRow(grid, row) {
    var dataItem = grid.dataItem(row);

    var rowChildren = $(row).children('td[role="gridcell"]');

    for (var i = 0; i < grid.columns.length; i++) {

        var column = grid.columns[i];
        var template = column.template;
        var cell = rowChildren.eq(i);

        if (template !== undefined) {
            var kendoTemplate = kendo.template(template);

            // Render using template
            cell.html(kendoTemplate(dataItem));
        } else {
            var fieldValue = dataItem[column.field];

            var format = column.format;
            var values = column.values;

            if (values !== undefined && values !== null) {
                // use the text value mappings (for enums)
                for (var j = 0; j < values.length; j++) {
                    var value = values[j];
                    if (value.value === fieldValue) {
                        cell.html(value.text);
                        break;
                    }
                }
            } else if (format !== undefined) {
                cell.html(kendo.format(format, fieldValue));
            } else {
                cell.html(fieldValue);
            }
        }
    }
}

function setFilterDefaultContains(e) {
    e.container.data("kendoPopup").bind("open", function () {
        var beginOperator = e.container.find("[data-role=dropdownlist]:eq(0)").data("kendoDropDownList");
        if (beginOperator) {
            beginOperator.value("contains");
            if (beginOperator.value()) { beginOperator.trigger("change"); } else { beginOperator.value(beginOperator.options.value); }
        } 
    });  
}

function showErrorMessage(e) {
    if (e.errors) {
        var message = '@Resources.Errors\n';
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + '\n';
                });
            }
        });
        alert(message);
    }
} 

function sitejs_kendoddlOptions(options) {
    return {
    filter: "contains",
    minLength: 2,
    dataTextField: "Text",
    dataValueField: "Value",  
    height: 300,

    optionLabel: {Value:"-1", Text: options.placeholder},
    optionLabelTemplate: "<span class='ddloptionlabel'>#:data.Text#</span>",    
    valueTemplate:"<span class='ddlvaluetemplate' title='" +options.placeholder+ "'>#:data.Text#</span>", 
    headerTemplate: options.headerTemplate,
    noDataTemplate: options.noDataTemplate
    } 
};

//clear validation on reset button clicked
(function ($) { 
    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {
        var $form = this.closest('form');

        //reset jQuery Validate's internals
        $form.validate().resetForm();

        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();

        $form.find(".input-validation-error")
        .removeClass("input-validation-error")
        
        return $form;
    };
})(jQuery);