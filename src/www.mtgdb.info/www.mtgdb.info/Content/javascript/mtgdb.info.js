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
    var opts = {
        lines: 9, // The number of lines to draw
        length: 8, // The length of each line
        width: 10, // The line thickness
        radius: 20, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 24, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#000', // #rgb or #rrggbb or array of colors
        speed: 1, // Rounds per second
        trail: 90, // Afterglow percentage
        shadow: true, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: 'auto', // Top position relative to parent in px
        left: 'auto' // Left position relative to parent in px
    };

    var target = document.getElementById('card_' + cardId);
    var spinner = new Spinner(opts).spin(target);

    var jqxhr = $.post( "/cards/" + cardId + "/amount/" + amount) 
    .done(function( data ) {
        $('#' + cardId).val(data);

        if(amount > 0)
        {
            $('#img_' + cardId).attr('class', 'owned');
        }
        else
        {
            $('#img_' + cardId).attr('class', 'dontown');
        }
            
        if($('#value_' + cardId).length != 0)
        {
            $('#value_' + cardId).text(amount);
        }
    })
    .fail(function() {
        alert( "Could not update card amount. Please try again later." );
    })
    .always(function() {
        spinner.stop();
    }); 
};

function format(set) {
    if (!set.id) return set.text; // optgroup
    return "<img class='flag' src='https://api.mtgdb.info/content/set_images/symbols/" + set.id.toLowerCase() + 
                                                                "_sym.png'/>" + " " + set.text;
}

$(document).ready(function() { $("#set_list").select2({
    matcher: function(term, text) { return text.toUpperCase().indexOf(term.toUpperCase())==0; },
    formatResult: format,
    formatSelection: format,
    escapeMarkup: function(m) { return m; }
});});

function go()
{
    window.location="/sets/" + document.getElementById("set_list").value
}

   

