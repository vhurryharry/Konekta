
import { IBreadcrumbItem, IBreadcrumbStyles } from 'office-ui-fabric-react/lib/Breadcrumb';
import { ILabelStyles } from 'office-ui-fabric-react/lib/Label';
import { Link } from 'react-router-dom'
import React from 'react';
import { DefaultButton, IButtonStyles } from 'office-ui-fabric-react';

export const breadcrumbHeaderStyle: Partial<ILabelStyles> = {
    root: {
        marginTop: '10px',
        marginLeft: '5px',
        fontSize: "20px",
        selectors: {
            '&:not(:first-child)': {
                marginTop: 24
            }
        }
    }
};

export const breadcrumbStyle: IBreadcrumbStyles = {
    root: {
        marginTop: '10px',
        marginBottom: '-10px'
    },
    chevron: {},
    item: {
        fontSize: "14px"
    },
    itemLink: {
        fontSize: "14px"
    },
    list: {},
    listItem: {},
    overflow: {},
    overflowButton: {}
}

interface BreadcrumbItemsData {
    path: string;
    items: IBreadcrumbItem[];
}

export const breadcrumbItemsData: BreadcrumbItemsData[] = [
    {
        path: '',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/' },
            { text: 'Unknown Page', key: 'error', isCurrentItem: true }
        ]
    },
    {
        path: '/',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/', isCurrentItem: true }
        ]
    },
    {
        path: '/matter',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/' },
            { text: 'Matter', key: 'matter', isCurrentItem: true }
        ]
    },
    {
        path: '/pexa/create-workspace',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/' },
            { text: 'Create PEXA Workspace', key: 'f1', isCurrentItem: true }
        ]
    },
    {
        path: '/calculators/settlement',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/' },
            { text: 'Calculators', key: 'f1' },
            { text: 'Settlement Calculator', key: 'f2', isCurrentItem: true }
        ]
    },
    {
        path: '/globalx/property-information',
        items: [
            { text: 'Konekta', key: 'Konekta', href: '/' },
            { text: 'GlobalX', key: 'f1' },
            { text: 'Property Information', key: 'f2', isCurrentItem: true }
        ]
    }
];

export default function breadcrumbItems(path: string): IBreadcrumbItem[] {

    const itemForPath = breadcrumbItemsData.slice().reverse().find(item => path.includes(item.path));
    if (itemForPath === undefined)
        return breadcrumbItemsData[0].items;

    return itemForPath.items;
}

export function onRenderItem(item: IBreadcrumbItem | undefined): JSX.Element {
    const breadcrumbButtonStyle: IButtonStyles = {
        root: {
            border: "none",
            padding: "0px 0px"
        }
    }

    if (item && item.href) {
        return (
            <Link to={item.href}>
                <DefaultButton text={item && item.text} styles={breadcrumbButtonStyle} />
            </Link>
        )
    } else {
        return (
            <DefaultButton text={item && item.text} styles={breadcrumbButtonStyle} />
        )
    }
}