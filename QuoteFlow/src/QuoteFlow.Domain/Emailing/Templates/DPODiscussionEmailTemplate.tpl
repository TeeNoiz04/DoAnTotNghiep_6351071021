<p>{{ model.recipient_full_name }},
<p>A new discussion message has been added to the distributor purchase order below:</p>

<h4>DPO Information</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">DPO No</td>
            <td style="width: 70%">{{ model.distributor_purchase_order.no }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Buyer Name</td>
            <td style="width: 70%">{{ model.distributor_purchase_order.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Confirm Date</td>
            <td>{{ model.distributor_purchase_order.confirm_date }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.distributor_purchase_order.material_type }}</td>
        </tr>
         <tr>
            <td class="text-bold">Note</td>
            <td>{{ model.distributor_purchase_order.note }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Amount</td>
            <td style="width: 70%">{{ model.distributor_purchase_order.total_amount | math.format "N0" }}</td>
        </tr>
        
    </table>
</div>

<h4>Discussion Message</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 20%">From</td>
            <td style="width: 80%">{{ model.sender_full_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Date</td>
            <td>{{ model.formatted_sent_date }}</td>
        </tr>
        <tr>
            <td class="text-bold">Message</td>
            <td>{{ model.comment }}</td>
        </tr>
    </table>
</div>

<p>Please log in to the system to view the complete discussion thread and respond if needed.</p>

    