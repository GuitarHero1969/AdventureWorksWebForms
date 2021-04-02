
$(function () {

    $('#btnSalesOrderLine').on('click', function () {
        addSalesOrderLine();
    });
});

// keep track of the number of order lines...
var lineIndex = 0;

function removeSalesOrderLine() {

    $('.plcLine').eq(stringToInt(lineIndex - 1)).remove();

    if (lineIndex > 0) {
        lineIndex--;
    }
}

// adds a new line of html input elements...
function addSalesOrderLine() {

    $('#salesOrderLinesContainer').append(buildHtml());

    loadCategories(lineIndex);

    // event listeners...
    $('#salesOrderLinesContainer').find('.clsCategories:eq(' + lineIndex + ')').on('change', function () {
        updateProducts(lineIndex, this);
    });

    $('#salesOrderLinesContainer').find('.btn-danger:eq(' + lineIndex + ')').on('click', function () {
        removeSalesOrderLine();
    });

    $('#salesOrderLinesContainer').find('.clsSubcategories:eq(' + lineIndex + ')').on('change', function () {
        updateProducts(lineIndex, this);
    });

    $('#salesOrderLinesContainer').find('.clsProducts:eq(' + lineIndex + ')').on('change', function () {
        updateProducts(lineIndex, this);
    });

    // clsQuantity onchange =\"updateLineTotal('" + lineIndex + "', this)\"
    $('#salesOrderLinesContainer').find('.clsQuantity:eq(' + lineIndex + ')').on('change', function () {
        updateProducts(lineIndex, this);
    });

    $('#salesOrderLinesContainer').find('.clsTotal:eq(' + lineIndex + ')').on('change', function () {
        updateOrderTotal();
    });


    // onblur=\"updateRow('" + lineIndex + "', this)\" oninput=\"validity.valid||(value='');\"


    lineIndex++;
}

function updateProducts(row, srcElem) {

    var lstPrice = 0.0;
    var qnt = 0.0;
    var orderLineTotal = 0.0;

    if ($(srcElem).hasClass('clsCategories')) {
        loadSubCategories(srcElem);

        $('.clsProducts:eq(' + row + ')').find('option').remove();
        $('.clsProducts:eq(' + row + ')').append($('<option></option').val(0).html('-- Select --'));
    }

    if ($(srcElem).hasClass('clsSubcategories')) {

        loadProducts(srcElem);

        // reset the List Price if Sub-Category is changed...
        $(srcElem).parents('.plcLine').find('.clsPrice').val("0.00");
    }

    // set the list price for the selected product item
    if ($(srcElem).hasClass('clsProducts')) {

        var productId = Number.parseInt($(srcElem).val());

        for (listPrice of listPrices) {
            if (Number.parseInt(listPrice['ProductID']) === productId) {
                $(srcElem).parents('.plcLine').find('.clsPrice').val(listPrice['ListPrice']);
            }
        }

        lstPrice = $(srcElem).parents('.plcLine').find('.clsPrice').val();

        var salesLines = $(srcElem).parents('#salesOrderLinesContainer')[0].children;

        for (line of salesLines) {
            // TODO: accumulate line totals and add to order total...
            console.log('line ', line);
        }

        $('#txtOrderTotal').val(lstPrice);
        $('#txtOrderTotal').prop('readonly', true);       
    }

    // the quantity changed...
    if ($(srcElem).hasClass('clsQuantity')) {

        lstPrice = $(srcElem).parents('.plcLine').find('.clsPrice').val();
        qnt = $(srcElem).parents('.plcLine').find('.clsQuantity').val();

        orderLineTotal = qnt * lstPrice;
        
        $(srcElem).parents('.plcLine').find('.clsTotal').val(orderLineTotal);
    }
}


function loadCategories(lineIndex) {

    $('.clsCategories:eq(' + lineIndex + ')').empty();
    $('.clsCategories:eq(' + lineIndex + ')').append($('<option></option').val(0).html('-- Select --'));

    fetch('SalesOrders_Editing.aspx/GetProductCategories', { method: 'GET', headers: { 'Content-Type': 'application/json' } })
        .then(res => res.json())
        .then(data => data.d.forEach(item => {
            $('.clsCategories:eq(' + lineIndex + ')').append($('<option></option').val(item.ProductCategoryID).html(item.Name));
        })).catch((err) => {
            console.log("Error retrieving categories", err);
        });

    $('.clsCategories:eq(' + lineIndex.toString() + ')').parents().find($('[data-loadspinner]')).hide().remove();
}

function loadSubCategories(ProductCategoryID) {

    var target = $(ProductCategoryID).parent().parent().find('.clsSubcategories');
    var productCategoryID = $(ProductCategoryID).val();

    $(target).find('option').remove();
    $(target).append($('<option></option').val(0).html('-- Select --'));
    
    fetch('SalesOrders_Editing.aspx/GetProductSubcategories?' + new URLSearchParams({ productCategoryID: productCategoryID }),
        { method: 'GET', headers: { 'Content-Type': 'application/json' } })
        .then(res => res.json())
        .then(data => data.d.forEach(item => {
            $(target).append($('<option></option>').val(item.ProductSubcategoryID).html(item.Name));
        }));
        
}

function loadProducts(ProductSubcategoryID) {

    var target = $(ProductSubcategoryID).parent().next().find('.clsProducts');
    var productSubcategoryID = $(ProductSubcategoryID).val();
    listPrices.length = 0;

    if ($(target).selectedIndex >= 0) {
        selectedValue = $(target).val();
    }

    $(target).find('option').remove();
    $(target).append($('<option></option').val(0).html('-- Select --'));

    fetch('SalesOrders_Editing.aspx/GetProducts?' + new URLSearchParams({ productSubcategoryID: productSubcategoryID }),
        { method: 'GET', headers: { 'Content-Type': 'application/json' } })
        .then(res => res.json())
        .then(data => data.d.forEach(item => {
            $(target).append($('<option></option>').val(item.ProductID).html(item.Name));
            listPrices.push(item);
        }));

}

function updateOrderTotal() {

    var accumulatedTotal = 0.0;

    console.log('update order ');

    $('#salesOrderLinesContainer .plcLine').each(function (i, rowHtml) {
        accumulatedTotal += parseFloat($(rowHtml).find('.clsTotal').val());
    });

    // $('#txtOrderTotal').val(accumulatedTotal.toString());
}

function updateDetail(srcElem) {

    var target = $(srcElem).parent().parent().parent().parent();

    var productPrice = 0.0;
    productPrice = $(target).find('.clsPrice').val();

    var discount = 0.0;
    discount = stringToDecimal($(target).find('.clsDiscount').val());

    var qty = 0;
    qty = $(target).find('.clsQuantity').val();

    var total = 0.0;
    total = (productPrice - discount) * qty;
    $(target).find('.clsTotal').val(total);

    // updateOrderTotal();
}

function resetFieldValues() {

    $('.clsCategories:eq(' + lineIndex + ')').empty();
    $('.clsCategories:eq(' + lineIndex + ')').append($('<option></option').val(0).html('-- Select --'));
    $('.clsPrice:eq(' + lineIndex + ')').empty();
}

// "create a sales order line object from the form values..."
function buildSalesOrderJSON() {

    var plcLine = {};
    var plcLines = [];

    $('#salesOrderLinesContainer .plcLine').each(function () {

        var row = {};
        row.salesorderlineid = $(this).data('indx');
        row.productid = $(this).find('.clsProducts :selected').val();
        row.productpackageid = $(this).find('.clsPackages :selected').val();
        row.productpackageconditionid = $(this).find('.clsCondition :selected').val();
        row.price = $(this).find('.clsPrice').val();
        row.quantity = $(this).find('.clsQuantity').val();
        row.discount = $(this).find('.clsDiscount').val();
        row.vat = $(this).find('.clsTaxCode :selected').val();
        row.totalprice = $(this).find('.clsTotal').val();
        plcLines.push(row);
    });

    plcLine.plcLineCollection = plcLines;
}

// #region AJAX calls to code-behind web methods

// C# 3.5 and above will serialize all JSON responses into a variable 'd'
// When the server sends a JSON response it will have a signature similar to this:
// { "d" : { "variable" : "value" }}



var listPrices = [];



// This replaces asp.net's form postback
function submitForm(plcLine) {

    var jsonisedVal = jsonToString("salesOrder", plcLine);

    $.ajax({
        method: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: "SalesOrders_Editing.aspx/UpdateSalesOrder",
        data: jsonisedVal,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(' submitForm(plcLine) ', errorThrown);
        },
        success: function (response) {
            console.log('form successfully submitted! ', response.data);
        },
        beforeSend: function () {

        }
    });
}
// end #region

// #region Helper functions

function jsonToString(param, jObject) {
    var jsonObj;
    jsonObj = "{\"" + param + "\": \"" + JSON.stringify(jObject).replace(/\"/g, "\\\"") + "\"}";
    return jsonObj;
}

function stringToDecimal(inputVal) {
    return Number.parseFloat(inputVal).toFixed(2);
}

function stringToInt(inputVal) {
    return Number.parseInt(inputVal);
}

// end #region

// #region Dynamic HTML

function buildHtml() {

    var plcLine = $([
        "<div class=\"plcLine p-1 row\" data-indx=" + lineIndex + ">",          
            "<div class=\"col-sm-2\">",
                "<select class=\"form-control clsCategories\"></select>",
            "</div>",
            "<div class=\"col-sm-2\">",
                "<select class=\"form-control clsSubcategories\" data-control=\"subCategories\"></select>",
            "</div>", 
            "<div class=\"col-sm-2\">",
                "<select class=\"form-control clsProducts\" data-control=\"products\"></select>",  
            "</div>",
            "<div class=\"col\">",
                "<input type=\"text\" class=\"form-control clsPrice\" data-control=\"price\">",
            "</div>",                   
            "<div class=\"col\">",
                "<input type=\"number\" min=\"1\" max=\"10\" value=\"1\" class=\"form-control clsQuantity\" data-control=\"quantity\">",
            "</div>",
            "<div class=\"col\">",
                "<input type=\"text\" class=\"form-control clsDiscount\" data-control=\"discount\" value=\"0.00\"/>",
            "</div>",
            "<div class=\"col\">",
                "<select class=\"form-control clsTaxCode\" data-control=\"tax\"></select>",
            "</div>",                                                          
            "<div class=\"col\">",
                "<input type=\"text\" class=\"form-control clsTotal\" data-control=\"total\"/>",
            "</div>",
            "<div class=\"col\">",
                "<input type=\"button\" class=\"btn btn-sm btn-danger\" value=\"Remove\" data-toggle=\"tooltip\" title=\"Remove Item\"/>",
            "</div>",                                       
        "</div>",

        "<div data-loadspinner=" + lineIndex + ">",
            "<i class=\"fas fa-spinner fa-spin\"></i>",
        "</div>"
    ].join("\n"));

    return plcLine;
}

// end #region



// "<div class=\"row clsLoadSpinner\"><div class=\"col-sm-1\" data-control=\"loadspinner\"><img src=\"/js/lightbox/images/loading.gif\"></div></div>"

// Storing Data
// sessionStorage.setItem('name', 'Matt West');

// Retrieving Data
// var name = sessionStorage.getItem('name');

// Deleting Data
// sessionStorage.removeItem('name');

// Retrieving an Item Key
// sessionStorage.key(n);

// Clearing the Datastore
// sessionStorage.clear();