<p>{{ model.recipient_full_name }},
<p>A new discussion message has been added to the price offer below:</p>

<h4>Price Offer Information</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">Price Offer Code</td>
            <td style="width: 70%">{{ model.price_offer.price_offer_code }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Project Name</td>
            <td style="width: 70%">{{ model.price_offer.project_name }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Buyer Name</td>
            <td style="width: 70%">{{ model.price_offer.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.price_offer.material_type }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Standard</td>
            <td style="width: 70%">{{ model.price_offer.total_standard | math.format "N0" }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Offer</td>
            <td style="width: 70%">{{ model.price_offer.total_offer | math.format "N0"}}</td>
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

