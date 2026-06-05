{{ if model.status == "REJECTED" }}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The psi has been rejected</p>
{{ else if model.status == "APPROVED" }}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The psi has been aprroved</p>
{{ else }}
    <p>Dear Approver(s),</p>
    <p>A new psi is waiting for your action</p>
{{ end }}

<h4>PSI</h4>
<div class="table-container">
    <table border="1">
        
        <tr>
            <td class="text-bold" style="width: 30%">File Name</td>
            <td style="width: 70%">{{ model.file_name }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">PSI Code</td>
            <td style="width: 70%">{{ model.psi_code }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Finance Year</td>
            <td style="width: 70%">{{ model.financial_year }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.material_type }}</td>
        </tr>
        <tr>
            <td class="text-bold">Note</td>
            <td>{{ model.note }}</td>
        </tr>
    </table>
</div>

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

<p>Click <a href="{{ model.approval_route }}">here</a> to take action.</p>