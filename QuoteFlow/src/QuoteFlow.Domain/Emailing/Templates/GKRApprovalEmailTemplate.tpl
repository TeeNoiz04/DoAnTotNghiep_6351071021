{{ if model.action == "Submitted" }}
    <p>Dear Approver,</p>
    <p>A new goods keeping request has been submitted for your approval.</p>
{{ else if model.action == "Approved" }}
    {{ if model.is_last_route }}
        <p>Dear Mr/Ms {{ model.gkr.creator_name }},</p>
        <p>The goods keeping request has been fully approved.</p>
    {{ else }}
        <p>Dear Approver,</p>
        <p>A goods keeping request has been approved and requires your approval.</p>
    {{ end }}
{{ else if model.action == "Rejected" }}
    <p>Dear Mr/Ms {{ model.gkr.creator_name }},</p>
    <p>The goods keeping request has been rejected.</p>
{{ end }}

<h4>GKR Information</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">GKR No</td>
            <td style="width: 70%">{{ model.gkr.no }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Buyer Name</td>
            <td style="width: 70%">{{ model.gkr.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Expiration Date</td>
            <td>{{ model.gkr.expiration_date }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.gkr.material_type }}</td>
        </tr>
         <tr>
            <td class="text-bold">Note</td>
            <td>{{ model.gkr.note }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Amount</td>
            <td style="width: 70%">{{ model.gkr.total_amount | math.format "N0" }}</td>
        </tr>

    </table>
</div>

<p>Click <a href="{{ model.approval_route }}">here</a> to take action.</p>
