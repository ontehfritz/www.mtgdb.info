function updateTotal(n)
{
    var amount = parseInt($('#total').text());
    amount = amount + n;
    $('#total').text(amount);
};

function updateUnique(n)
{
    var amount = parseInt($('#unique').text());
    amount = amount + n;
    $('#unique').text(amount);
};

function updateSetCount(set, n)
{
    var amount = parseInt($('#' + set).text());
    amount = amount + n;
    $('#' + set).text(amount);
};

function updateBlockCount(block, n)
{
    var amount = parseInt($('#' + block).text());
    amount = amount + n;
    $('#' + block).text(amount);
};

function minusCard(cardId, block, set)
{
    var amount = parseInt($('#' + cardId).val());
    if(amount > 0)
    {
        amount = amount - 1;
        set_amount(cardId, amount);
        updateTotal(-1);

        if(amount == 0)
        {
            updateUnique(-1);
            if($('#active_block').length != 0)
            {
                var block = $('#active_block').val();
                updateBlockCount(block, -1);
            }  

            if($('#active_set').length != 0)
            {
                var set = $('#active_set').val();
                updateSetCount(set, -1);
            }  
        }
    }
};

function addCard(cardId)
{
    var amount = parseInt($('#' + cardId).val());

    if(amount == 0)
    {
        updateUnique(1);
        if($('#active_block').length != 0)
        {
            var block = $('#active_block').val();
            updateBlockCount(block, 1);
        } 
         
        if($('#active_set').length != 0)
        {
            var set = $('#active_set').val();
            updateSetCount(set, 1);
        }  
    }

    amount = amount + 1;
    set_amount(cardId, amount);
    updateTotal(1);
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
        $('#img_' + cardId).attr('class', 'owned');

        if($('#value_' + cardId).length != 0)
        {
            $('#value_' + cardId).text(amount);
        }
    })
    .fail(function() {
        alert( "Could not update card amount. Please try again later." );
    })
    .always(function() {
        
    }); 
};

