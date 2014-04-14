$('.popit').popover({
    html: true,
    trigger: 'hover',
    placement: 'top',
    animation: false
    })
    .on('show.bs.popover', function (e) {
        var $this = $(this);

        // Currently hovering popover
        $this.data("hoveringPopover", true);

        // If it's still waiting to determine if it can be hovered, don't allow other handlers
        if ($this.data("waitingForPopoverTO")) {
            //alert('hide');
            e.stopImmediatePropagation();
        }
    })
    .on('hide.bs.popover', function (e) {
       var $this = $(this);
      
        // If timeout was reached, allow hide to occur
        if ($this.data("forceHidePopover")) {
            $this.data("forceHidePopover", false);
            return true;
        }

        // Prevent other `hide` handlers from executing
        e.stopImmediatePropagation();

        // Reset timeout checker
        clearTimeout($this.data("popoverTO"));

        // No longer hovering popover
        $this.data("hoveringPopover", false);

        // Flag for `show` event
        $this.data("waitingForPopoverTO", true);

        // In 1500ms, check to see if the popover is still not being hovered
        $this.data("popoverTO", setTimeout(function () {
            // If not being hovered, force the hide
            if (!$this.data("hoveringPopover")) {
                $this.data("forceHidePopover", true);
                $this.data("waitingForPopoverTO", false);
                $this.popover("hide");
            }
        }, 100));

        // Stop default behavior
        return false;
});


$(function() {
        $( ".rdate" ).datepicker({ dateFormat: 'yy-mm-dd' }).val();
});

$("#add_ruling").click(function(){
    var count = parseInt($("#ruling_count").val()) + 1;
    $("#ruling_count").val(count);

    $('#rulings > tbody:last').prepend('<tr id="ruling_' + count + '"><td><input type="text" class="rdate" name="Rulings[' +  count
    + '].ReleasedAt" value="" placeholder="Enter date" required /></td><td><textarea rows="4" cols="50" name="Rulings[' + count + 
    '].Rule" required></textarea></td> <td><span class="glyphicon glyphicon-minus-sign" style="cursor:pointer;" onclick="removeRuling(\'ruling_' + count + '\');"></span></td></tr>');
    $( ".rdate" ).datepicker({ dateFormat: 'yy-mm-dd' }).val();
}); 

function removeRuling(id)
{
    //alert(id);
    $('#' + id).remove();
    return false;
}

$("#add_format").click(function(){
    var count = parseInt($("#format_count").val()) + 1;
    $("#format_count").val(count);

    $('#formats > tbody:last').prepend('<tr id="format_' + count + '"><td><input type="text" name="Formats[' +  count
    + '].Name" value="" required /></td><td><input type="text" name="Formats[' + count + 
    '].Legality" value="" /></td> <td><span class="glyphicon glyphicon-minus-sign" style="cursor:pointer;" onclick="removeFormat(\'format_' + count + '\');"></span></td></tr>');
}); 

function removeFormat(id)
{
    //alert(id);
    $('#' + id).remove();
    return false;
}

$(document).ready(function() { 
	$("#set_list").select2({
    	matcher: function(term, text) { return text.toUpperCase().indexOf(term.toUpperCase())==0; },
    	formatResult: format,
    	formatSelection: format,
    	escapeMarkup: function(m) { return m; }
	});

	$('.card').hover(function(){
		$('.card-amount').stop().fadeOut();
		$(this).find('.card-amount').stop().fadeIn();
	},function(){
		$(this).find('.card-amount').stop().fadeOut();
	});

	$('.card-amount').hide();

	$('.label-legality').tooltip();

	$('.search-result').popover({trigger:'hover',html:true});
});

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

function go()
{
    window.location="/sets/" + document.getElementById("set_list").value
}

