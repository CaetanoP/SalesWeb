import { Toaster } from 'sonner';
import './globals.css';

import { Analytics } from '@vercel/analytics/react';

export const metadata = {
  title: 'SalesWebDashboard',
  description:
    'A dashboard for managing your sales, customers, and products.',
};

export default function RootLayout({
  children
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className="flex min-h-screen w-full flex-col">{children}</body>
      <Analytics />
      <Toaster richColors/>
    </html>
  );
}
