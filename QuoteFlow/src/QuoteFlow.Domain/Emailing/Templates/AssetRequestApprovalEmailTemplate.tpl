
{{ if model.status == "REJECTED" }}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The Asset Request has been rejected</p>

{{ else if model.status == "WAIT_FOR_RETURN" && model.action == "Approved"}}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The Asset Request has been aprroved</p>

{{ else if model.status == "CLOSED"}}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The Asset Request has been closed</p>


{{ else if model.status == "APPROVED"}}
    <p>Dear Mr/Ms {{ model.creator_name }},
    <p>The Asset Request has been approved</p>

{{ else }}
    <p>Dear Approver(s),</p>
    <p>A new Asset Request is waiting for your action</p>
{{ end }}


<h4>Asset Request</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">Asset Request No</td>
            <td style="width: 70%">{{ model.request_no }}</td>
        </tr>
       
        <tr>
            <td class="text-bold">Asset Type</td>
            <td>{{ model.request_type }}</td>
        </tr>

        <tr>
            <td class="text-bold">Requester</td>
            <td>{{ model.creator_name }}</td>
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

