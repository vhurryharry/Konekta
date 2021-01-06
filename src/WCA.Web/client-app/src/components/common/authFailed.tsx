import * as React from 'react'
import { MessageBar, MessageBarType } from 'office-ui-fabric-react'

export default class AuthFailed extends React.Component {
    public render() {
        return (
            <MessageBar messageBarType={MessageBarType.error}>
                Sorry, something went wrong. Please <a href="https://support.konekta.com.au/support/tickets/new" target="_blank" rel="noopener noreferrer">submit a ticket</a> and we will be back to you soon.
            </MessageBar>
        )
    }
}