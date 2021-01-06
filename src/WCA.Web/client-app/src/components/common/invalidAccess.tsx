import * as React from 'react'

import { Text, Image, Stack } from 'office-ui-fabric-react';
import { MessageBar, MessageBarType } from 'office-ui-fabric-react';

export default class InvalidAccess extends React.Component {
    public render() {
        return (
            <Stack styles={{ root: { overflow: 'hidden', width: '100%', padding: '30px' } }}>
                <MessageBar messageBarType={MessageBarType.error}>
                    <Text variant="medium">You have attempted to access Konekta from an invalid location. Please use the Konekta link from within an Actionstep matter.</Text>
                </MessageBar><p></p>

                <Stack.Item>
                    <Image src="/images/konekta-as-integration.png" alt={"Actionstep - Konekta Integration"} width="100%" />
                </Stack.Item>
            </Stack>
        );
    }
}