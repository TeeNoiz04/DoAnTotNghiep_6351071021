{{ if model.key_account.status == "REJECTED" }}
    <p>Dear Mr/Ms {{ model.key_account.creator_name }},
    <p>The key account has been rejected</p>
{{ else if model.key_account.status == "APPROVED" }}
    <p>Dear Mr/Ms {{ model.key_account.creator_name }},
    <p>The key account has been aprroved</p>
{{ else }}
    <p>Dear Approver(s),</p>
    <p>A new key account is waiting for your action</p>
{{ end }}

<h4>Key Account</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">KA Code</td>
            <td style="width: 70%">{{ model.key_account.key_account_code }}</td>
        </tr>
        <tr>
            <td class="text-bold">KA Name</td>
            <td>{{ model.key_account.key_account_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Buyer Name</td>
            <td>{{ model.key_account.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Tax Code</td>
            <td>{{ model.key_account.tax_code }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.key_account.material_type }}</td>
        </tr>
        <tr>
            <td class="text-bold">KA Buyer Type Name</td>
            <td>{{ model.key_account.key_account_buyer_type_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">KA Buyer Class Name</td>
            <td>{{ model.key_account.key_account_buyer_class_name }}</td>
        </tr>
        {{ if model.key_account.key_account_class_name }}
            <tr>
                <td class="text-bold">KA Type Name</td>
                <td>{{ model.key_account.key_account_type_name }}</td>
            </tr>
            <tr>
                <td class="text-bold">KA Class Name</td>
                <td>{{ model.key_account.key_account_class_name }}</td>
            </tr>
        {{ end }}
        
    </table>
</div>
{{ if model.key_account.status != "REJECTED" && model.key_account.status != "APPROVED" }}
    <h4>Approval History:</h4>
    <div class="table-container">
        <table border="1">
            <tr style="background-color: lightgrey;">
                <th>Name</th>
                <th>Role</th>
                <th>Action</th>
                <th>Date</th>
                <th>Comment</th>
            </tr>
        
            {{ for history in model.approval_histories }}
            <tr>
                <td>{{ history.approver_full_name }}</td>
                <td>{{ history.approver_role_name }}</td>
                <td>{{ history.action }}</td>
                <td>{{ history.action_date }}</td>
                <td>{{ history.note }}</td>
            </tr>
            {{ end }}
        </table>
    </div>
{{ end }}

{{ if model.key_account.status != "REJECTED" && model.key_account.status != "APPROVED"}}
    <p>Click <a href="{{ model.approval_route }}">here</a> to take action.</p>
{{ end }}