function minusCard(cardId)
{
    var amount = parseInt($('#' + cardId).val());
    if(amount > 0)
    {
        amount = amount - 1;
        set_amount(cardId, amount);
    }
};

function addCard(cardId)
{
    var amount = parseInt($('#' + cardId).val());
    amount = amount + 1;
    set_amount(cardId, amount);
};

function changeAmount(cardId)
{
    var isInt = /^\d+$/;
    if(isInt.test($('#' + cardId).val()))
    {
        var amount = parseInt($('#' + cardId).val());
        set_amount(cardId, amount);
    }
};
  
function set_amount(cardId, amount)
{
    var jqxhr = $.post( "/cards/" + cardId + "/amount/" + amount) 
    .done(function( data ) {
        $('#' + cardId).val(data);
    })
    .fail(function() {
        alert( "Could not update card amount. Please try again later." );
    })
    .always(function() {
        
    }); 
};